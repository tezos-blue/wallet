using Microsoft.AppCenter.Analytics;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Identities
{
	using Cryptography;
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RestoreIdentityPage : ContentPage
	{
		public RestoreIdentityPage(RestoreIdentityVM restoreVM)
		{
			InitializeComponent();

			BindingContext = restoreVM;

			_Restore.Committed += OnClickImport;
			_Restore.UnlockMethod = restoreVM.UnlockMethod;
		}

		RestoreIdentityVM VM => BindingContext as RestoreIdentityVM;

		private async void OnClickImport(Passphrase passphrase)
		{
			Analytics.TrackEvent("Wallet | Restore | Restore");

			if (await VM.Restore(passphrase))
			{
				// Show the imported identity
				Close(true);
			}
			else
			{
				_Restore.RetryAfterFail();
			}
		}

		private void OnClickCancel(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Restore | Cancel");
			Close(false);
		}

		void Close(bool restored) => UserInterface.OnIdentityRestored(restored);
	}
}