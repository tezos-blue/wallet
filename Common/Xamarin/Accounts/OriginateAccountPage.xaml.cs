using Microsoft.AppCenter.Analytics;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Origination
{
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OriginateAccountPage : ContentPage
	{
		public OriginateAccountPage()
		{
			InitializeComponent();
		}

		CreateAccountVM VM => BindingContext as CreateAccountVM;

		private void OnClickCreate(object sender, EventArgs e)
		{
			Analytics.TrackEvent("Wallet | Originate | Commit");
			VM.Create();
			Navigation.PopToRootAsync();
		}

		private void OnTransferAll(object sender, EventArgs e)
		{
			Analytics.TrackEvent("Wallet | Originate | TransferAll");
			VM.TransferAll();
		}
	}
}