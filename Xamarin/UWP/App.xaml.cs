using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SLD.Tezos.Client.UWP
{
	using OS;

	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	sealed partial class App : Application
	{
		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			this.InitializeComponent();
			this.Suspending += OnSuspending;

			ApplicationCenter.BackupProviders.Add(new WindowsShareBackup());
		}

		/// <summary>
		/// Invoked when the application is launched normally by the end user.  Other entry points
		/// will be used such as when the application is launched to open a specific file.
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
			{
				//this.DebugSettings.EnableFrameRateCounter = true;
			}
#endif
			TouchAssembliesOnce();

			LaunchMainUI(e);
		}

		protected override async void OnFileActivated(FileActivatedEventArgs args)
		{
			Trace.WriteLine("File activated");

			base.OnFileActivated(args);

			var file = args.Files.First() as StorageFile;

			string json = await FileToJson(file);

			LaunchMainUI(args);

			ApplicationCenter.Receive(json);
		}

		protected override async void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
		{
			ShareOperation shareOperation = args.ShareOperation;

			var formats = shareOperation.Data.AvailableFormats.ToArray();
			if (shareOperation.Data.Contains(StandardDataFormats.StorageItems))
			{
				shareOperation.ReportStarted();

				var storageItems = await shareOperation.Data.GetStorageItemsAsync();

				var firstFile = storageItems.First() as StorageFile;

				string json = await FileToJson(firstFile);

				shareOperation.ReportDataRetrieved();

				LaunchMainUI(args);

				ApplicationCenter.Receive(json);
			}
		}

		private static async System.Threading.Tasks.Task<string> FileToJson(StorageFile file)
		{
			var stream = await file.OpenSequentialReadAsync();

			var reader = new StreamReader(stream.AsStreamForRead(), Encoding.UTF8);

			var json = await reader.ReadToEndAsync();
			return json;
		}

		private void TouchAssembliesOnce()
		{
			void Touch<T>()
			{
				var assembly = typeof(T).Assembly;
				Trace.WriteLine(assembly.FullName);
			}

			Touch<System.IO.Compression.CompressionLevel>();
			Touch<Connections.CloudConnection>();
			Touch<ITelemetry>();
			Touch<Serialization.Document>();
		}

		private void LaunchMainUI(IActivatedEventArgs e)
		{
			Trace.WriteLine("Launch MainUI");

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active
			if (!(Window.Current.Content is Frame rootFrame))
			{
				// Create a Frame to act as the navigation context and navigate to the first page
				rootFrame = new Frame();

				rootFrame.NavigationFailed += OnNavigationFailed;

				Xamarin.Forms.Forms.Init(e);

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					//TODO: Load state from previously suspended application
				}

				// Place the frame in the current Window
				Window.Current.Content = rootFrame;
			}

			if (rootFrame.Content == null)
			{
				// When the navigation stack isn't restored navigate to the first page,
				// configuring the new page by passing required information as a navigation
				// parameter
				rootFrame.Navigate(typeof(MainPage), null /*e.Arguments*/);
			}
			// Ensure the current window is active
			Window.Current.Activate();
		}

		/// <summary>
		/// Invoked when Navigation to a certain page fails
		/// </summary>
		/// <param name="sender">The Frame which failed navigation</param>
		/// <param name="e">Details about the navigation failure</param>
		private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}

		/// <summary>
		/// Invoked when application execution is being suspended.  Application state is saved
		/// without knowing whether the application will be terminated or resumed with the contents
		/// of memory still intact.
		/// </summary>
		/// <param name="sender">The source of the suspend request.</param>
		/// <param name="e">Details about the suspend request.</param>
		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			//TODO: Save application state and stop any background activity
			deferral.Complete();
		}
	}
}