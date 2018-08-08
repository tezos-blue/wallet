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
	public partial class DelegatePage : ContentPage
	{
		public DelegatePage()
		{
			InitializeComponent();
		}

		private DelegateVM VM => BindingContext as DelegateVM;

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			var clipboard = DependencyService.Get<IClipboard>();

			var text = await clipboard.GetCopiedText();

			if (text != null && VM.Engine.IsValidAccountID(text.Trim()))
			{
				_Paste.IsEnabled = true;
			}
		}



		private void OnClickCommit(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Delegate | Commit");
			VM.Commit();

			Navigation.PopToRootAsync();
		}

		private void OnClickCancel(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Delegate | Cancel");
			Navigation.PopAsync();
		}

		private async Task OnPasteAccountID(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Delegate | Paste");
			var clipboard = DependencyService.Get<IClipboard>();

			_DelegateID.Text = await clipboard.GetCopiedText();
		}

		private void OnDelegateAll(object sender, EventArgs e)
		{
			Analytics.TrackEvent("Wallet | Delegate | DelegateAll");
			VM.TransferAll();
		}

	}
}