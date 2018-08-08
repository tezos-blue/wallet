using Microsoft.AppCenter.Analytics;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Identities
{
	using Model;
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IdentitiesPage : ContentPage
	{
		public IdentitiesPage()
		{
			InitializeComponent();
		}

		public void OnClickNew(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Identities | New");

			Navigation.PushAsync(new SelectIdentityTypePage());
		}

		private void OnIdentitySelected(object sender, SelectedItemChangedEventArgs e)
		{
			Analytics.TrackEvent("Wallet | Identities | Select");

			if (!(e.SelectedItem is IdentityVM identity))
			{
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}

			MainVM.Current.CurrentIdentity = identity;

			((ListView)sender).SelectedItem = null; //uncomment line if you want to disable the visual selection state.

			Navigation.PopToRootAsync();
		}
	}
}