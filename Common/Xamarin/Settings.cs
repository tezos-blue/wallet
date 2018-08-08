using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Resources;
using Xamarin.Forms;

[assembly: NeutralResourcesLanguage("en")]
namespace SLD.Tezos.Client
{
	public static class Settings
	{
		public const int PINLength = 4;

		public static string InstanceID { get; private set; }

		public static string PushToken
		{
			get => UserSettings.GetValueOrDefault("PushToken", null);
			set => UserSettings.AddOrUpdateValue("PushToken", value);
		}

		public static TargetPlatform TargetPlatform
		{
			get
			{
				switch (Device.RuntimePlatform)
				{
					case Device.iOS:
						return TargetPlatform.iOS;

					case Device.Android:
						return TargetPlatform.Android;

					case Device.UWP:
						return TargetPlatform.UWP;

					default:
						return TargetPlatform.Unsupported;
				}
			}
		}

		public static TimeSpan TimeToShowInfoMessages { get; internal set; } = TimeSpan.FromSeconds(1.25);
		public static TimeSpan TimeToShowAlertMessages { get; internal set; } = TimeSpan.FromSeconds(3);

		private static ISettings UserSettings => CrossSettings.Current;

		internal static void InitializeInstanceID(Guid? appcenterID)
		{
			if (appcenterID.HasValue)
			{
				Tracer.Trace(typeof(Settings), $"InstanceID: {appcenterID.Value} (AppCenter)");

				InstanceID = appcenterID.Value.ToString();
			}
			else
			{
				InstanceID = UserSettings.GetValueOrDefault("InstanceID", null);

				if (InstanceID == null)
				{
					InstanceID = Guid.NewGuid().ToString();

					Tracer.Trace(typeof(Settings), $"InstanceID: {InstanceID} (new)");

					UserSettings.AddOrUpdateValue("InstanceID", InstanceID);
				}
				else
				{
					Tracer.Trace(typeof(Settings), $"InstanceID: {InstanceID} (local storage)");
				}
			}
		}
	}
}