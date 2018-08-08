using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Service
{
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReleaseNotesPage : ContentPage
	{
		public ReleaseNotesPage(ReleaseNotesVM releaseNotesVM)
		{
			InitializeComponent();
		}

		public void OnDismiss(object sender, EventArgs args)
		{
			UserInterface.OnReleaseNotesRead();
		}
	}
}