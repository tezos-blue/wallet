using Android.Content;
using Android.Support.V4.Content;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SLD.Tezos.Client.OS
{
	using Localization;

	internal class AndroidSendBackup : IBackupIdentity
	{
		private static MainActivity mainActivity;
		public string NameKey => "Backup";

		public string DescriptionKey => throw new NotImplementedException();

		public async Task<bool?> Backup(string identityID, byte[] backupData)
		{
			var cache = mainActivity.CacheDir;
			var filename = new Java.IO.File(cache, $"{identityID}.blue").AbsolutePath;

			using (var stream = File.Create(filename))
			{
				await stream.WriteAsync(backupData, 0, backupData.Length);
			}

			var file = new Java.IO.File(filename);

			var email = new Intent(Intent.ActionSend);
			email.PutExtra(Intent.ExtraSubject, Localizer.Translate("BackupFor") + " " + identityID);
			email.PutExtra(Intent.ExtraText, Localizer.Translate("BackupMailBody"));
			email.PutExtra(Intent.ExtraStream, FileProvider.GetUriForFile(mainActivity, "de.lautereck.tezos.blue", file));

			email.SetType("message/rfc822");
			email.SetFlags(ActivityFlags.GrantReadUriPermission);

			mainActivity.StartActivity(Intent.CreateChooser(email, Localizer.Translate("BackupVia")));

			return null;
		}

		internal static void Init(MainActivity mainActivity)
		{
			AndroidSendBackup.mainActivity = mainActivity;

			if (!ApplicationCenter.BackupProviders.Any(p => p is AndroidSendBackup))
			{
				ApplicationCenter.BackupProviders.Add(new AndroidSendBackup());
			}
		}
	}
}