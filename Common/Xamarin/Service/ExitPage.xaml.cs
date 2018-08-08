using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Service
{
	using Localization;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExitPage : ContentPage
	{
		private ExitPage(string message)
		{
			InitializeComponent();

			_Message.Text = message;
		}

		internal static ExitPage CreateFromError(string errorID)
			=> new ExitPage(Localizer.Translate(errorID));

		internal static Page CreateFromException(UnhandledExceptionEventArgs e)
			=> new ExitPage(e.ExceptionObject.ToString());

		private void OnClickExit(object sender, EventArgs e)
		{
			App.Restart();
		}
	}
}