using Foundation;
using MessageUI;
using System;
using System.Threading.Tasks;

namespace SLD.Tezos.Client.OS
{
	using Localization;
	using SLD.Tezos.Client.iOS;
	using UIKit;

	internal class AppleMailBackup : IBackupIdentity
	{
		public string NameKey => "BackupByMail";

		public string DescriptionKey => throw new NotImplementedException();

		public async Task<bool?> Backup(string identityID, byte[] backupData)
		{
			var sync = new SyncEvent();

			var mailController = new MFMailComposeViewController();

			mailController.SetSubject(Localizer.Translate("BackupFor") + " " + identityID);
			mailController.SetMessageBody(Localizer.Translate("BackupMailBody"), false);

			var nsData = NSData.FromArray(backupData);

			mailController.AddAttachmentData(nsData, "application/json", $"{identityID}.blue");

			mailController.Finished += (object s, MFComposeResultEventArgs args) => {

				switch (args.Result)
				{
					case MFMailComposeResult.Saved:
					case MFMailComposeResult.Sent:
						sync.SetComplete();
						break;
					default:
						sync.SetComplete(false);
						break;
				}

				args.Controller.DismissViewController(true, null);
			};

			UIApplication
				.SharedApplication
				.KeyWindow
				.RootViewController
				.PresentViewController(mailController, true, null);

			return await sync.WhenComplete;
		}

		static AppDelegate appDelegate;

		internal static void Initialize(AppDelegate appDelegate, NSDictionary options)
		{
			AppleMailBackup.appDelegate = appDelegate;

			if (MFMailComposeViewController.CanSendMail)
			{
				ApplicationCenter.BackupProviders.Add(new AppleMailBackup());
			}
		}
	}
}