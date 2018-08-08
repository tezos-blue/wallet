using Microsoft.AppCenter.Analytics;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client
{
	using Cryptography;
	using Localization;
	using Model;
	using Protocol;
	using Security;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ApprovalPage : ContentPage
	{
		private Approval approval;

		public ApprovalPage(Approval approval)
		{
			InitializeComponent();
			BindingContext = this.approval = approval;

			approval.TimedOut += OnTimedOut;

			// Accounts and Fees
			_SourceAccount.Text = GetAccountName(approval.Task.SourceID);
			_TransferAmount.Amount = approval.Task.TransferAmount;
			_NetworkFeeAmount.Amount = approval.Task.NetworkFee;
			_ServiceFeeAmount.Amount = approval.Task.ServiceFee;
			_TotalFees.Amount = approval.Task.Fees;

			if (approval.Task.StorageFee > 0)
			{
				_StorageFeeAmount.Amount = approval.Task.StorageFee;
			}
			else
			{
				_StorageFee.IsVisible = false;
			}

			_TotalAmount.Amount = approval.Task.TotalAmount;

			// Identity Stereotype
			var identityType = IdentityType.Get(approval.Signer.Stereotype);
			_Commit.UnlockMethod = identityType.UnlockMethod;
			_Commit.NeedsCredentials = approval.Signer.IsLocked;

			_Commit.Committed += OnCommit;

			// Task type
			switch (approval.Task)
			{
				case CreateContractTask originate:
					_Title.Text = Localizer.Translate("SignOrigination");
					_Destination.IsVisible = false;
					break;

				case TransferTask transfer:
					_Title.Text = Localizer.Translate("SignTransfer");
					_DestinationAccount.Text = GetAccountName(transfer.DestinationID);
					break;

				default:
					throw new NotImplementedException();
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			_Commit.InitializeFocus();
		}

		private async void OnCommit(Passphrase passphrase)
		{
			Analytics.TrackEvent("Wallet | Approval | Sign");

			approval.Passphrase = passphrase;

			var result = await approval.Approve(true);

			switch (result)
			{
				case SigningResult.InvalidCredentials:
					_Commit.RetryAfterFail();
					break;

				case SigningResult.Signed:
				case SigningResult.Cancelled:
				case SigningResult.Timeout:
				case SigningResult.ProviderFailed:
				default:
					//App.Keyboard.Hide();
					await Navigation.PopModalAsync();
					break;
			}
		}

		private string GetAccountName(string accountID)
		{
			var account = App.Engine[accountID];

			return account != null ? account.Name : accountID;
		}

		private void OnTimedOut()
		{
			Analytics.TrackEvent("Wallet | Approval | Timeout");
			Navigation.PopModalAsync();
		}

		private void OnClickCancel(object sender, EventArgs e)
		{
			Analytics.TrackEvent("Wallet | Approval | Cancel");
			approval.Approve(false);
			Navigation.PopModalAsync();
		}

		private void OnClickFees(object sender, EventArgs e)
		{
			_FeeDetails.IsVisible = true;
			_FeeSummary.IsVisible = false;
		}
	}
}