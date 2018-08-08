using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

namespace SLD.Tezos.Client.OS
{
	using Localization;
	using Windows.Storage.Streams;

	internal class WindowsShareBackup : IBackupIdentity
	{
		private IStorageItem backup;

		private DataTransferManager dataTransferManager;

		public WindowsShareBackup()
		{
		}

		public string NameKey => "Backup";

		public string DescriptionKey => throw new NotImplementedException();

		public async Task<bool?> Backup(string identityID, byte[] backupData)
		{
			if (dataTransferManager == null)
			{
				// Have to do it here because in json file share scenario, data transfer manager cannot be initialized earlier.
				Initialize();
			}

			backup = await PrepareBackup(identityID, backupData);

			DataTransferManager.ShowShareUI();

			return null;
		}

		private async void Initialize()
		{
			await ApplicationCenter.UIReady.WhenComplete;

			dataTransferManager = DataTransferManager.GetForCurrentView();
			dataTransferManager.DataRequested += OnDataRequested;
		}

		private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
		{
			var request = args.Request;

			request.Data.SetStorageItems(new[] { backup });

			request.Data.Properties.Title = Localizer.Translate("BackupFor") + " " + identityID;

			request.Data.Properties.ApplicationListingUri = new Uri(ApplicationInfo.UWPStoreLink);
			request.Data.Properties.ApplicationName = ApplicationInfo.ProductName;
			request.Data.Properties.Description = Localizer.Translate("BackupMailBody");
			request.Data.Properties.FileTypes.Add(".blue");
			request.Data.Properties.Square30x30Logo =
				RandomAccessStreamReference.CreateFromUri(
					new Uri("ms-appx:///Assets/Logo30.png", UriKind.Absolute));
			//request.Data.Properties.Thumbnail =
			//	RandomAccessStreamReference.CreateFromUri(
			//		new Uri("ms-appx:///Assets/Logo30.png", UriKind.Absolute));
		}

		string identityID;

		private async Task<IStorageItem> PrepareBackup(string identityID, byte[] backupData)
		{
			this.identityID = identityID;
			var tempFolder = ApplicationData.Current.TemporaryFolder;

			var file = await tempFolder.CreateFileAsync($"{identityID}.blue", CreationCollisionOption.ReplaceExisting);

			await FileIO.WriteBytesAsync(file, backupData);

			return file;
		}
	}
}