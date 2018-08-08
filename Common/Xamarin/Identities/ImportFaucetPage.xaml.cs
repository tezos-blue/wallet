using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Identities
{
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ImportFaucetPage : ContentPage
	{
		public ImportFaucetPage (ImportFaucetVM faucetVM)
		{
			InitializeComponent ();

			BindingContext = VM = faucetVM;
		}

		ImportFaucetVM VM;

		void OnClickImport(object sender, EventArgs args)
		{
			VM.Import();

			Navigation.PopModalAsync();
		}

		void OnClickCancel(object sender, EventArgs args)
		{
			VM.Cancel();

			Navigation.PopModalAsync();
		}

		protected override bool OnBackButtonPressed()
		{
			VM.Cancel();

			return base.OnBackButtonPressed();
		}

		void Close(bool restored) => UserInterface.OnIdentityRestored(restored);

	}
}