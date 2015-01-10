using System;
using Storage;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;
using Project_Xelda;

namespace Storage
{
	[Serializable]
	public struct SaveGame
	{
		public int week;
		public int day;
		public int Event;
        public Vector2 apos;
        public bool avis;
        public string aname;
        public Vector2 bpos;
        public bool bvis;
        public string bname;

        public CharacterStruct player;

        public DateTime date;
	}

	public class SaveGameStorage
	{
		public void Save(SaveGame sg, int saveno, bool auto)

		{
			StorageDevice device = StorageDevice.ShowStorageDeviceGuide();

			// Open a storage container
			StorageContainer container = device.OpenContainer("my games/Project Xelda");

			// Get the path of the save game
			string filename;
			if (auto == false)
				filename = Path.Combine(container.Path, "savegame"+saveno+".xml");
			else
				filename = Path.Combine(container.Path, "auto_savegame.xml");

			if (File.Exists (filename))
				File.Delete (filename);
			// Open the file, creating it if necessary
			FileStream stream = File.Open(filename, FileMode.OpenOrCreate);

			// Convert the object to XML data and put it in the stream
			XmlSerializer serializer = new XmlSerializer(typeof(SaveGame));
			serializer.Serialize(stream, sg);

			// Close the file
			stream.Close();

			// Dispose the container, to commit changes
			container.Dispose();
		}

		public SaveGame Load(int saveno, bool auto)
		{
			SaveGame ret = new SaveGame();

			StorageDevice device = StorageDevice.ShowStorageDeviceGuide();

			// Open a storage container
			StorageContainer container = device.OpenContainer("my games/Project Xelda");

			// Get the path of the save game
			string filename;
			if (auto == false)
				filename = Path.Combine(container.Path, "savegame"+saveno+".xml");
			else
				filename = Path.Combine(container.Path, "auto_savegame.xml");

			// Check to see if the save exists
			if (!File.Exists(filename))
				// Notify the user there is no save           
				return ret;

			// Open the file
			FileStream stream = File.Open(filename, FileMode.OpenOrCreate,
				FileAccess.Read);

			// Read the data from the file
			XmlSerializer serializer = new XmlSerializer(typeof(SaveGame));
			ret = (SaveGame)serializer.Deserialize(stream);

			// Close the file
			stream.Close();

			// Dispose the container
			container.Dispose();

			return ret;
		}
	}
}