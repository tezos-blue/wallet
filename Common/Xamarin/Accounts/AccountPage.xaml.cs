using System;
using Microsoft.AppCenter.Analytics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Accounts
{
	using Origination;
	using OS;
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountPage : ContentPage
	{
		public AccountPage()
		{
			InitializeComponent();

			_Entries.MeasureInvalidated += OnEntriesMeasure;

		}

		private void OnEntriesMeasure(object sender, EventArgs e)
		{
			if (Bounds.Width < 300)
			{
				foreach (AccountEntryView entryView in _Entries.Children)
				{
					entryView.Shrink();
				}
			}
		}

		private AccountVM VM => BindingContext as AccountVM;

		private void OnClickTransfer(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Account | Transfer");
			var task = new TransferVM(VM.Parent) { SelectedSource = VM.Account };

			Navigation.PushAsync(new TransferPage { BindingContext = task });
		}

		private void OnClickOriginate(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Account | Originate");
			var task = new CreateAccountVM(VM.Parent) { SelectedSource = VM.Account };

			Navigation.PushAsync(new OriginateAccountPage { BindingContext = task });
		}

		private void OnCopyAccountID(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Account | Copy");
			var clipboard = DependencyService.Get<IClipboard>();

			clipboard.CopyText(VM.Account.AccountID);
		}

		private void OnClickDelegate(object sender, EventArgs e)
		{
			Analytics.TrackEvent("Wallet | Account | Delegate");
			var task = new DelegateVM(VM.Parent) { SelectedSource = VM.Account };

			Navigation.PushAsync(new DelegatePage { BindingContext = task });
		}
	}
}