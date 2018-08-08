using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SLD.Tezos.Client
{
	using Localization;
	using OS;

	public partial class App : Application
	{
		internal static Engine Engine;
		private static IConnectNetwork connector;

#if DEBUG
		private const Network ConnectTo = Network.Test;
#else
		private const Network ConnectTo = Network.Default;
#endif

		public App()
		{
			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

			// Handle when your app starts
			connector = DependencyService.Get<IConnectNetwork>();

			Analytics.TrackEvent("Wallet | Application starting");

			MainPage = UserInterface.Splash;

			InitializeComponent();
		}

		public static string InstanceID => Settings.InstanceID;

		internal static void Restart()
		{
			(Current as App).LaunchEverything(connector);
		}

		#region Lifetime

		protected override async void OnStart()
		{
#if DEBUG
			Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
#endif

			// Localization
			// On some platforms, localization has to be tweaked. (iOS, Android)
			var osLocalize = DependencyService.Get<ILocalize>();
			if (osLocalize != null)
			{
				Localizer.Culture = osLocalize.GetCurrentCulture();
			}

			// Start process _______________________________________________________________________
			var stopWatch = Stopwatch.StartNew();

			// InstanceID
			Settings.InitializeInstanceID(await AppCenter.GetInstallIdAsync());

			// UI Thread affinity
			Engine.SynchronizeContext(SynchronizationContext.Current);

			try
			{
				await LaunchEverything(connector);
			}
			catch (Exception e)
			{
				Telemetry.TrackException("LaunchEverything failed.", e);
				throw;
			}

			stopWatch.Stop();

			Telemetry.TrackEvent("Started",
				"ServiceState", Engine.ServiceState.ToString(),
				"Duration", stopWatch.Elapsed.ToSafeString());
		}

		protected override void OnSleep()
		{
			Analytics.TrackEvent("Wallet | App | Suspend");

			Engine?.Suspend();
		}

		protected override async void OnResume()
		{
			Analytics.TrackEvent("Wallet | App | Resume");

			if (Engine != null)
			{
				await Engine.Resume();
			}
		}

		private async Task LaunchEverything(IConnectNetwork connector)
		{
			Trace.WriteLine($"Connecting to {ConnectTo}");

			ApplicationInfo.ServiceInfo = await connector.GetServiceInfo(ConnectTo);

			Engine = connector.Connect(ConnectTo, null, InstanceID, ApplicationInfo.Version);

			Telemetry.Initialize(Engine.Connection as ITelemetry);

			// Check version
			var isNewVersion = await CheckNewVersion();

			// Bind UI
			UserInterface.Launch(Engine, isNewVersion);

			// Start Engine
			await Engine.Start();
		}

		private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Telemetry.TrackException("Unhandled", e.ExceptionObject as Exception,
				"IsTerminating", e.IsTerminating.ToString()
				);
		}

		private async Task<bool> CheckNewVersion()
		{
			var lastVersion = ApplicationInfo.LastApplicationVersion;

			if (lastVersion == null || string.Compare(lastVersion, ApplicationInfo.Version) < 0)
			{
				await OnVersionChanged(lastVersion);
				ApplicationInfo.LastApplicationVersion = ApplicationInfo.Version;

				return true;
			}

			return false;
		}

		private async Task OnVersionChanged(string previousVersion)
		{
			if (string.Compare(previousVersion, "0.2.9") <= 0)
			{
				// Start with no identities
				Analytics.TrackEvent("Wallet | Purge");
				await Engine.PurgeAll();
			}
		}

		#endregion Lifetime

		#region OS

		internal static IKeyboard Keyboard = DependencyService.Get<IKeyboard>();

		#endregion OS
	}
}