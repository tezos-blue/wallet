using Microsoft.AppCenter.Analytics;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Service
{
	using Localization;
	using OS;
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BackupPage : ContentPage
	{
		public BackupPage()
		{
			InitializeComponent();

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

		private IdentityVM VM => BindingContext as IdentityVM;

		private async void OnClickBackup(IBackupIdentity backupProvider)
		{
			Analytics.TrackEvent($"Wallet | Identity | Backup | {backupProvider.NameKey}");

			await VM.Backup(backupProvider);
		}

		private void OnToggleMarked(object sender, EventArgs args)
		{
		}

		private async void OnClickDone(object sender, EventArgs args)
		{
			await Navigation.PopAsync();
		}
	}
}