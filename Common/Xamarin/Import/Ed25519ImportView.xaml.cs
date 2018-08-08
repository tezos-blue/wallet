using Microsoft.AppCenter.Analytics;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Import
{
	using OS;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Ed25519ImportView : ContentView
	{
		public Ed25519ImportView()
		{
			InitializeComponent();
		}

		private Ed25519Import Import => BindingContext as Ed25519Import;

		public async void OnPasteKey(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Ed25519 Import | Paste");
			var clipboard = DependencyService.Get<IClipboard>();

			_Ed25519.Text = (await clipboard.GetCopiedText()).Trim();
		}

		protected async override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (Import != null)
			{
				var clipboard = DependencyService.Get<IClipboard>();

				var text = await clipboard.GetCopiedText();

				if (text != null && Import.IsValidEd25519(text.Trim()))
				{
					_Paste.IsEnabled = true;
				}
			}
		}
	}
}