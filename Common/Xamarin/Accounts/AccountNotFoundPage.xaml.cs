using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter.Analytics;

namespace SLD.Tezos.Client.Accounts
{
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountNotFoundPage : ContentPage
	{
		public AccountNotFoundPage()
		{
			InitializeComponent();
		}

		private AccountVM VM => BindingContext as AccountVM;

		private void OnClickDelete(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Retired | Delete");
			VM.Delete();

			Navigation.PopAsync();
		}
	}
}