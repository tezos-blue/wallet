using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System.IO;
using System.Text;

namespace SLD.Tezos.Client
{
	using Notifications;
	using OS;

	[Activity(
		LaunchMode = LaunchMode.SingleTask,
		Label = Client.ApplicationInfo.ProductName, 
		Icon = "@drawable/icon", 
		Theme = "@style/MainTheme", 
		MainLauncher = true, 
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
	)]
	[IntentFilter(
		new[] { Intent.ActionView },
		Categories = new[]
		{
			Intent.CategoryDefault,
			Intent.CategoryBrowsable,
		},
		DataScheme = "file",
		DataMimeType = "application/*",
		DataPathPattern = ".*\\\\.blue",
		Icon = "@drawable/icon"
	)]
	[IntentFilter(
		new[] { Intent.ActionView },
		Categories = new[]
		{
			Intent.CategoryDefault,
			Intent.CategoryBrowsable,
		},
		DataScheme = "content",
		DataMimeType = "application/*",
		DataPathPattern = ".*\\\\.blue",
		Icon = "@drawable/icon"
	)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication(new App());

			if (!PushNotifications.Check(this))
			{
				Finish();
			}

			AndroidKeyboard.Init(this);
			AndroidClipboard.Init(this);
			AndroidSendBackup.Init(this);

			// File opened with this launch?
			//string action = Intent.Action;
			//string type = Intent.Type;

			//if (Intent.ActionView.Equals(action) && !string.IsNullOrEmpty(type))
			//{
			//	Android.Net.Uri fileUri = Intent.Data;

			//	string json = File.ReadAllText(fileUri.Path, Encoding.UTF8);

			//	ApplicationCenter.Receive(json);
			//}

			CheckData(Intent);
		}

		protected override void OnNewIntent(Intent intent)
		{
			base.OnNewIntent(intent);

			CheckData(intent);
		}

		private void CheckData(Intent intent)
		{
			var data = intent.Data;

			if (data != null)
			{
				var t = Intent.Type;

				var stream = ContentResolver.OpenInputStream(data);

				var reader = new StreamReader(stream, Encoding.UTF8);

				var json = reader.ReadToEnd();

				ApplicationCenter.Receive(json);
			}
		}
	}
}