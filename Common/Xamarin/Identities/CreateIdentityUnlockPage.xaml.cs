using Microsoft.AppCenter.Analytics;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Identities
{
	using Localization;
	using Model;
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateIdentityUnlockPage : ContentPage
	{
		private bool usePIN;

		public CreateIdentityUnlockPage()
		{
			InitializeComponent();
		}

		private CreateIdentityVM VM => BindingContext as CreateIdentityVM;

		public void OnClickLogin(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Create Identity | Unlock");

			VM.CreateAndUnlock();

			UserInterface.OnIdentityAvailable();
		}

		public void OnTextChanged(object sender, EventArgs args)
		{
			if (usePIN)
			{
				if (_UnlockEntry.Text.Length == Settings.PINLength)
				{
					if (VM.CanLogin)
					{
						OnClickLogin(null, null);
					}
					else
					{
						_UnlockEntry.Text = string.Empty;
					}
				}
			}
			else
			{
				VM.FirePropertyChanged(nameof(CreateIdentityVM.CanLogin));
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			switch (VM.IdentityType.UnlockMethod)
			{
				case UnlockMethod.PIN:
					_Login.IsVisible = false;
					_UnlockEntry.HorizontalTextAlignment = TextAlignment.Center;
					_UnlockEntry.Placeholder = Localizer.Translate("EnterPIN");
					_UnlockEntry.Keyboard = Keyboard.Telephone;
					usePIN = true;
					break;

				default:
				case UnlockMethod.Passphrase:
					break;
			}

			_UnlockEntry.Focus();
		}
	}
}