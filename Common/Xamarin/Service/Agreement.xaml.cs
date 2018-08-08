using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Service
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Agreement : ContentPage
	{
		public Agreement()
		{
			InitializeComponent();
		}

		public void OnAgree(object sender, EventArgs args)
		{
			ApplicationInfo.SignAgreement();

			UserInterface.OnLicenseAgreed();
		}

		public void OnQuit(object sender, EventArgs args)
		{
			Application.Current.Quit();
		}
	}
}