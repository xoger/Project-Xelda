using System;
using Storage;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;

namespace Storage
{
	[Serializable]
	public struct Settings
	{
		public bool widescreen;
		public bool fullscreen;
		[NonSerialized]
		public int DontKeep;
	}

	public class SettingsStorage
	{
		public void Save(Settings sg)

		{
			StorageDevice device = StorageDevice.ShowStorageDeviceGuide();

			// Open a storage container
			StorageContainer container = device.OpenContainer("my games/Project Xelda");

			// Get the path of the save game
			string filename;
			filename = Path.Combine(container.Path, "settings.xml");

			if (File.Exists (filename))
				File.Delete (filename);

			// Open the file, creating it if necessary
			FileStream stream = File.Open(filename, FileMode.OpenOrCreate);

			// Convert the object to XML data and put it in the stream
				XmlSerializer serializer = new XmlSerializer(typeof(Settings));
			serializer.Serialize(stream, sg);

			// Close the file
			stream.Close();

			// Dispose the container, to commit changes
			container.Dispose();
		}

		public Settings Load()
		{
			Settings ret = new Settings();

			StorageDevice device = StorageDevice.ShowStorageDeviceGuide();

			// Open a storage container
			StorageContainer container = device.OpenContainer("my games/Project Xelda");

			// Get the path of the save game
			string filename;
			filename = Path.Combine(container.Path, "settings.xml");

			// Check to see if the save exists
			if (!File.Exists(filename))
				// Notify the user there is no save           
				return ret;

			// Open the file
			FileStream stream = File.Open(filename, FileMode.OpenOrCreate,
				FileAccess.Read);

			// Read the data from the file
			XmlSerializer serializer = new XmlSerializer(typeof(Settings));
			ret = (Settings)serializer.Deserialize(stream);

			// Close the file
			stream.Close();

			// Dispose the container
			container.Dispose();

			return ret;
		}
	}
}