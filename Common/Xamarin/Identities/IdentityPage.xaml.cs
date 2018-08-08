using Microsoft.AppCenter.Analytics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Identities
{
	using Localization;
	using OS;
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IdentityPage : ContentPage
	{
		public IdentityPage()
		{
			InitializeComponent();
		}

		private IdentityVM VM => BindingContext as IdentityVM;

		protected override void OnAppearing()
		{
			base.OnAppearing();

			foreach (var backupProvider in ApplicationCenter.BackupProviders)
			{
				var button = new Button
				{
					Text = Localizer.Translate(backupProvider.NameKey),
				};

				button.Clicked += (s, e) => OnClickBackup(backupProvider);

				_BackupProviders.Children.Add(button);
			}
		}

		private async void OnClickBackup(IBackupIdentity backupProvider)
		{
			Analytics.TrackEvent($"Wallet | Identity | Backup | {backupProvider.NameKey}");

			await VM.Backup(backupProvider);
		}
	}
}