using Microsoft.AppCenter.Analytics;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Identities
{
	using Controls;
	using Import;
	using Model;
	using System.Threading.Tasks;
	using UI;
	using Localization;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ImportKeysPage : ContentPage
	{
		public ImportKeysPage(CreateIdentityVM vm)
		{
			BindingContext = VM = vm;
			InitializeComponent();

			switch (vm.KeySource)
			{
				default:
				case KeyImportSource.Fundraiser:
					//_Source.SelectedIndex = 0;
					Title = Localizer.Translate("ImportFundraiserWallet");
					_Panel.Content = new FundraiserImportView();
					break;
				case KeyImportSource.Brain:
					//_Source.SelectedIndex = 1;
					Title = Localizer.Translate("ImportBrainWallet");
					_Panel.Content = new BrainImportView();
					break;
				case KeyImportSource.Ed25519:
					//_Source.SelectedIndex = 2;
					_Panel.Content = new Ed25519ImportView();
					break;
			}
		}

		private CreateIdentityVM VM;

		public async void OnClickImport(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Import Keys | Import");

			var result = VM.Import();

			if (result.IsError)
			{
				VM.CancelImport();

				//_Source.IsVisible = false;
				_Import.IsVisible = false;
				_Panel.Content = new InfoView
				{
					TextID = result.ErrorID,
					InfoLevel = InfoLevel.Alert,
				};

				await Task.Delay(Settings.TimeToShowAlertMessages);
			}

			await Navigation.PopAsync();
		}

		//public void OnSourceChanged(object sender, EventArgs args)
		//{
		//	SetPanelContent();
		//}

		//private void SetPanelContent()
		//{
		//	var selected = _Source.SelectedItem?.ToString();

		//	Analytics.TrackEvent($"Wallet | Import Keys | Select {selected}");

		//	switch (selected)
		//	{
		//		case "Fundraiser":
		//			VM.KeySource = KeyImportSource.Fundraiser;
		//			_Panel.Content = new FundraiserImportView();
		//			break;

		//		case "Ed25519":
		//			VM.KeySource = KeyImportSource.Ed25519;
		//			_Panel.Content = new Ed25519ImportView();
		//			break;

		//		case "Brain":
		//			VM.KeySource = KeyImportSource.Brain;
		//			_Panel.Content = new BrainImportView();
		//			break;

		//		default:
		//			VM.KeySource = KeyImportSource.Generation;
		//			_Panel.Content = null;
		//			break;
		//	}
		//}

		protected override bool OnBackButtonPressed()
		{
			VM.CancelImport();
			return base.OnBackButtonPressed();
		}

	}
}