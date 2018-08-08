using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Xamarin.Forms;

namespace SLD.Tezos.Client
{
	using Documents;
	using FirstSteps;
	using Identities;
	using Security;
	using Serialization;
	using Service;
	using UI;

	internal static class UserInterface
	{
		public static SplashScreen Splash = new SplashScreen();
		private static Engine engine;
		private static bool isNewVersion;
		private static bool isLaunching;
		private static MainVM mainVM;

		internal static void OnIdentityRestored(bool restored)
		{
			if (isLaunching)
			{
				// Is current screen
				if (restored)
				{
					OnIdentityAvailable();
				}
				else
				{
					OnLaunchToIdentity();
				}
			}
			else
			{
				// Has been pushed as modal during live
				Synchronize(PopModalPage);

				if (restored)
				{
					Synchronize(ShowHome);
				}
			}
		}

		private static void OnDocumentReceived(object waitingDocument)
		{
			switch (waitingDocument)
			{
				case FaucetDocument faucet:
					PushModalPage(new ImportFaucetPage(new ImportFaucetVM(faucet)));
					break;

				case Document document:
					switch (document.DocumentType)
					{
						case DocumentType.IdentityBackup:
							PushModalPage(new RestoreIdentityPage(new RestoreIdentityVM(document)));
							break;

						default:
							Trace.WriteLine($"Unknown document type: {document.DocumentType}");
							break;
					}
					break;

				default:
					break;
			}
		}

		private static void GetApproval(Approval approval)
		{
			Screen.Navigation.PushModalAsync(new ApprovalPage(approval));
		}

		#region Sequence

		// Synchronize with Engine initialization
		private static readonly object syncEngineInitialized = new object();
		private static bool? isEngineInitialized;
		private static bool isBriefingComplete;

		internal static void Launch(Engine engine, bool isNewVersion)
		{
			Screen = new SplashScreen();

			context = SynchronizationContext.Current;
			isLaunching = true;

			UserInterface.engine = engine;
			UserInterface.isNewVersion = isNewVersion;

			engine.ApprovalRequired += GetApproval;
			engine.Initialized += OnEngineInitialized;

			mainVM = new MainVM(engine);

			// Initial screen
			if (!ApplicationInfo.IsLicensed)
			{
				// Must sign agreement to continue
				Screen = new Agreement();
			}
			else
			{
				OnLicenseAgreed();
			}
		}

		private static void OnEngineInitialized(bool success)
		{
			lock (syncEngineInitialized)
			{
				isEngineInitialized = success;
				OnRequiredComplete();
			}
		}

		internal static void OnLicenseAgreed()
		{
			// Release notes only if not launched with document
			if (ApplicationCenter.WaitingDocument == null && isNewVersion)
			{
				// Release Notes
				Screen = new ReleaseNotesPage(MainVM.Current.ReleaseNotes);
			}
			else
			{
				isBriefingComplete = true;
				OnRequiredComplete();
			}
		}

		internal static void OnReleaseNotesRead()
		{
			isBriefingComplete = true;
			OnRequiredComplete();
		}

		internal static void OnRequiredComplete()
		{
			// check for identities to be initialized
			lock (syncEngineInitialized)
			{
				if (isEngineInitialized.HasValue)
				{
					// Initialization is complete
					if (!isEngineInitialized.Value)
					{
						// Fatal, exit
						Screen = ExitPage.CreateFromError("Error_ServiceDown");
						return;
					}
				}
				else
				{
					// Still waiting for Engine to initialize
					Screen = new SplashScreen();
					return;
				}
			}

			// Check for briefing to be complete
			if (!isBriefingComplete)
			{
				return;
			}

			// Go
			OnReadyToEnter();
		}

		private static void OnReadyToEnter()
		{
			// Check for document on launch
			if (ApplicationCenter.WaitingDocument != null)
			{
				OnLaunchWithDocument(ApplicationCenter.WaitingDocument);
			}
			else
			{
				OnLaunchToIdentity();
			}

			// Start listening to incoming documents
			ApplicationCenter.DocumentReceived += OnDocumentReceived;
		}

		internal static void OnIdentityAvailable()
		{
			ShowHome();
		}

		private static void OnLaunchToIdentity()
		{
			if (engine.Identities.Any())
			{
				OnIdentityAvailable();
			}
			else
			{
				var vm = new CreateIdentityVM();

				Screen = new NavigationPage(new FirstStepsPage(vm));
			}
		}

		private static void OnLaunchWithDocument(object waitingDocument)
		{
			switch (waitingDocument)
			{
				case FaucetDocument faucet:
					Screen = new ImportFaucetPage(new ImportFaucetVM(faucet));
					break;

				case Document document:
					switch (document.DocumentType)
					{
						case DocumentType.IdentityBackup:
							Screen = new RestoreIdentityPage(new RestoreIdentityVM(document));
							break;

						default:
							Trace.WriteLine($"Unknown document type: {document.DocumentType}");
							OnLaunchToIdentity();
							break;
					}
					break;

				default:
					break;
			}
		}

		private static void ShowHome()
		{
			if (isLaunching)
			{
				homePage = new MainPage
				{
					BindingContext = mainVM,
				};

				var navPage = new NavigationPage(homePage);

				rootPage = new RootPage
				{
					MasterBehavior = MasterBehavior.Popover,

					Master = mainMenu = new MainMenu(navPage, () => rootPage.IsPresented = false)
					{
						Title = "...",
					},

					Detail = navPage,
				};

				Screen = rootPage;

				isLaunching = false;
			}
			else
			{
				PopToRoot();
			}

			homePage.Initialize(engine);
		}

		#endregion Sequence

		#region UI

		private static MainMenu mainMenu;
		private static SynchronizationContext context;
		private static MainPage homePage;
		private static RootPage rootPage;

		internal static Page Screen
		{
			get => Application.Current.MainPage;
			set => Application.Current.MainPage = value;
		}

		public static void PushPage(Page page)
			=> homePage.Navigation.PushAsync(page);

		public static void PushModalPage(Page page)
			=> Screen.Navigation.PushModalAsync(page);

		public static void PopModalPage()
			=> homePage.Navigation.PopModalAsync();

		public static void PopToRoot()
			=> homePage.Navigation.PopToRootAsync();

		private static void Synchronize(Action action)
		{
			context.Post(state => (state as Action).Invoke(), action);
		}

		#endregion UI
	}
}