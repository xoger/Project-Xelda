using System;

namespace Storage
{
	public class StorageDevice
	{
		public bool IsConnected
		{
			get
			{
				return true;
			}
		}

		public StorageContainer OpenContainer(string containerName)
		{
			return new StorageContainer(this,containerName);
		}

		public static StorageDevice ShowStorageDeviceGuide()
		{
			return new StorageDevice();
		}
	}
}