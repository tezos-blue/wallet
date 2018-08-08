using Microsoft.AppCenter.Analytics;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Import
{
	using OS;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BrainImportView : ContentView
	{
		public BrainImportView()
		{
			InitializeComponent();
		}

		private BrainImport Import => BindingContext as BrainImport;

		public async void OnPasteKey(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Brain Import | Paste");
			var clipboard = DependencyService.Get<IClipboard>();

			_Brain.Text = (await clipboard.GetCopiedText()).Trim();
		}

		//protected async override void OnBindingContextChanged()
		//{
		//	base.OnBindingContextChanged();

		//	if (Import != null)
		//	{
		//		var clipboard = DependencyService.Get<IClipboard>();

		//		var text = await clipboard.GetCopiedText();

		//		if (text != null && Import.IsValidBrain(text.Trim()))
		//		{
		//			_Paste.IsEnabled = true;
		//		}
		//	}
		//}
	}
}