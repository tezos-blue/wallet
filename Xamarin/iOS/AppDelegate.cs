using Foundation;
using System;
using System.IO;
using UIKit;
using Xamarin.Forms;

namespace SLD.Tezos.Client.iOS
{
	using Notifications;

	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			Forms.Init();

			LoadApplication(new App());

			PushNotifications.Initialize(options);
			Client.OS.AppleMailBackup.Initialize(this, options);

			return base.FinishedLaunching(app, options);
		}

		public override void OnActivated(UIApplication uiApplication)
		{
			//	base.OnActivated(uiApplication);
		}

		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
			var path = url.Path;

			var json = File.ReadAllText(path);

			File.Delete(path);

			ApplicationCenter.Receive(json);

			return true;
			//return base.OpenUrl(app, url, options);
		}

		#region Notification Overrides

		public override void RegisteredForRemoteNotifications(UIApplication app, NSData deviceToken)
		{
			PushNotifications.RegisterToken(deviceToken);
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			PushNotifications.ReportRegistrationError(error);
		}

		public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			PushNotifications.ProcessMessage(userInfo);
		}

		#endregion Notification Overrides
	}
}