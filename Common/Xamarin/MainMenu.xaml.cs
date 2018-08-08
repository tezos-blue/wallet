using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client
{
	using Identities;
	using Service;
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainMenu : ContentPage
	{
		NavigationPage nav;
		Action Close;



		public MainMenu (NavigationPage nav, Action close)
		{
			InitializeComponent ();

			this.nav = nav;
			this.Close = close;
		}

		private void GoHome(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Menu | Home");

			nav.PopToRootAsync();
			Close();
		}

		private void GoIdentities(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Menu | Identities");

			nav.PushAsync(new IdentitiesPage { BindingContext = MainVM.Current });
			Close();
		}

		private void GoSettings(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Menu | Settings");

			nav.PushAsync(new SettingsPage());
			Close();
		}

	}
}