using Microsoft.AppCenter.Analytics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Accounts
{
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectOwnDestinationPage : ContentPage
	{
		public TransferVM VM;

		public SelectOwnDestinationPage(TransferVM vm)
		{
			BindingContext = VM = vm;

			InitializeComponent();
		}

		private void OnAccountSelected(object sender, SelectedItemChangedEventArgs e)
		{
			Analytics.TrackEvent("Wallet | Destination | Select");
			if (e.SelectedItem == null)
			{
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}

			Navigation.PopModalAsync();
		}
	}
}