using Microsoft.AppCenter.Analytics;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Accounts
{
	using Localization;
	using OS;
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransferPage : ContentPage
	{
		public TransferPage()
		{
			InitializeComponent();
		}

		private TransferVM VM => BindingContext as TransferVM;

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			var clipboard = DependencyService.Get<IClipboard>();

			var text = await clipboard.GetCopiedText();

			if (text != null && VM.Engine.IsValidAccountID(text.Trim()))
			{
				_Paste.IsEnabled = true;
			}

			if (VM.SelectedDestination == null)
			{
				_SelectDestinationType.IsVisible = true;
				_ManualEntry.IsVisible = false;
			}
		}

		private void OnClickToOwn(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Transfer | Own");
			_SelectDestinationType.IsVisible = false;
			//_AvailableOwn.IsVisible = true;
			_ManualEntry.IsVisible = false;

			Navigation.PushModalAsync(new SelectOwnDestinationPage(VM));
		}

		private void OnClickToForeign(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Transfer | Foreign");
			_Destination.Text = Localizer.Translate("DestinationAccountID");
			_SelectDestinationType.IsVisible = false;
			//_AvailableOwn.IsVisible = false;
			_ManualEntry.IsVisible = true;
		}

		private void OnClickCommit(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Transfer | Commit");
			VM.Commit();

			Navigation.PopToRootAsync();
		}

		private void OnClickCancel(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Transfer | Cancel");
			Navigation.PopAsync();
		}

		private async Task OnPasteAccountID(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Transfer | Paste");
			var clipboard = DependencyService.Get<IClipboard>();

			_ManualAccountID.Text = await clipboard.GetCopiedText();
		}

		private void OnTransferAll(object sender, EventArgs e)
		{
			Analytics.TrackEvent("Wallet | Transfer | TransferAll");
			VM.TransferAll();
		}
	}
}