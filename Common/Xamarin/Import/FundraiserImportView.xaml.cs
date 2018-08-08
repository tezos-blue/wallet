using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Import
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FundraiserImportView : ContentView
	{
		public FundraiserImportView()
		{
			InitializeComponent();
		}

		private FundraiserImport Import => BindingContext as FundraiserImport;

		public void OnSelectGuess(object sender, EventArgs args)
		{
			Import.SelectGuess();
			_Current.Focus();
		}

		public void OnReset(object sender, EventArgs args)
		{
			Import.Reset();
			_Current.Focus();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (Import != null)
			{
				Import.PropertyChanged += (s, args) =>
				{
					switch (args.PropertyName)
					{
						case "IsSentenceComplete":
							if (Import.IsSentenceComplete)
							{
								_Mnemonic.IsVisible = false;
								_Credentials.IsVisible = true;
							}
							break;
					}
				}; 
			}
		}
	}
}