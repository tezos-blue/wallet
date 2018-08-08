using Microsoft.AppCenter.Analytics;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Identities
{
	using UI;
	using Localization;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateIdentityPage : ContentPage
	{
		string stereotype;

		public CreateIdentityPage(string stereotype)
		{
			this.stereotype = stereotype;

			InitializeComponent();

			switch (stereotype)
			{
				case "Light":
					break;

				case "Standard":
				default:
					Title = Localizer.Translate("CreateStandardIdentity");
					_PIN.IsVisible = false;
					_Passphrase.IsVisible = true;
					_EntryLabel.Text = Localizer.Translate("Passphrase");
					_Tip.TextID = "CreateStandardIdentityTip";
					break;
			}
		}

		private CreateIdentityVM VM => BindingContext as CreateIdentityVM;

		public void OnClickCreate(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Create Light | Create");

			Navigation.PushAsync(new CreateIdentityUnlockPage { BindingContext = VM });
		}

		public void OnClickRandom(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Create Light | Import");

			VM.KeySource = Model.KeyImportSource.Generation;
		}

		public void OnTextChanged(object sender, EventArgs args)
		{
			if (_PIN.Text != null && _PIN.Text.Length == Settings.PINLength)
			{
				Navigation.PushAsync(new CreateIdentityUnlockPage { BindingContext = VM });
			}

			VM.FirePropertyChanged(nameof(CreateIdentityVM.CanCreate));
		}

		private void OnClickImportFundraiser(object sender, EventArgs e)
		{
			Analytics.TrackEvent("Wallet | Create Identity | Import Fundraiser");

			VM.KeySource = Model.KeyImportSource.Fundraiser;

			Navigation.PushAsync(new ImportKeysPage(VM));
		}

		private void OnClickImportBrain(object sender, EventArgs e)
		{
			Analytics.TrackEvent("Wallet | Create Identity | Import Brain");

			VM.KeySource = Model.KeyImportSource.Brain;

			Navigation.PushAsync(new ImportKeysPage(VM));
		}

		private void OnClickImportEd25519(object sender, EventArgs e)
		{
			Analytics.TrackEvent("Wallet | Create Identity | Import Ed25519");

			VM.KeySource = Model.KeyImportSource.Ed25519;

			Navigation.PushAsync(new ImportKeysPage(VM));
		}
	}
}