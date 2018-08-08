using Microsoft.AppCenter.Analytics;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Identities
{
	using Model;
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectIdentityTypePage : ApplicationPage
	{
		public SelectIdentityTypePage()
		{
			InitializeComponent();

			_Selector.IdentityTypeSelected += OnIdentityTypeSelected;
		}

		private void OnIdentityTypeSelected(IdentityType type)
		{
			Debug.Assert(type != null);
			Analytics.TrackEvent($"Wallet | IdentityType | {type.Stereotype}");

			Page next = new CreateIdentityPage(type.Stereotype)
			{
				BindingContext = new CreateIdentityVM(type)
			};

			Navigation.PushAsync(next);
		}
	}
}