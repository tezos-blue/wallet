using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace SLD.Tezos.Client.OS
{
	using Localization;
	using Windows.Storage;
	using Windows.Storage.Provider;

	class WindowsFileBackup : IBackupIdentity
	{
		public string NameKey => "BackupByFile";

		public string DescriptionKey => throw new NotImplementedException();

		public async Task<bool?> Backup(string identityID, byte[] backupData)
		{
			try
			{
				var localFolder = ApplicationData.Current.RoamingFolder;

				var file = await localFolder.CreateFileAsync($"{identityID}.blue");

				//	// Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
				CachedFileManager.DeferUpdates(file);

				await FileIO.WriteBytesAsync(file, backupData);

				// Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
				// Completing updates may require Windows to ask for user input.
				FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);

				return true;
			}
			catch
			{
				return false;
			}

			//FileSavePicker picker = new FileSavePicker
			//{
			//	SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
			//	SuggestedFileName = $"{identityID}.blue"
			//};

			//StorageFile file = await picker.PickSaveFileAsync();
			//if (file != null)
			//{
			//	// Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
			//	CachedFileManager.DeferUpdates(file);
				
			//	// write to file
			//	await FileIO.WriteBytesAsync(file, backupData);

			//	// Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
			//	// Completing updates may require Windows to ask for user input.
			//	FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);

			//	return status == FileUpdateStatus.Complete;
			//}
			//else
			//{
			//	return false;
			//}
		}
	}
}
