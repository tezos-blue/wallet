using Microsoft.AppCenter.Analytics;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.FirstSteps
{
	using Identities;
	using Localization;
	using Model;
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FirstStepsPage : ContentPage
	{
		public FirstStepsPage(CreateIdentityVM vm)
		{
			InitializeComponent();

			vm.Name = Localizer.Translate("mine");

			BindingContext = VM = vm;

			_Selector.IdentityTypeSelected += OnIdentityTypeSelected;

			NavigationPage.SetHasBackButton(this, false);
		}

		public CreateIdentityVM VM { get; private set; }

		protected override void OnAppearing()
		{
			ApplicationCenter.UIReady.SetComplete();

			base.OnAppearing();
		}

		protected override bool OnBackButtonPressed()
		{
			// Cannot go back
			return true;
		}

		private void OnIdentityTypeSelected(IdentityType type)
		{
			Debug.Assert(type != null);
			Analytics.TrackEvent($"Wallet | FirstStep | {type.Stereotype}");

			Page next = new CreateIdentityPage(type.Stereotype);

			VM.ResetToType(type);

			next.BindingContext = VM;

			Navigation.PushAsync(next);
		}
	}
}