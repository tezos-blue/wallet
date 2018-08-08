using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SplashScreen : ContentPage
	{
		public SplashScreen()
		{
			InitializeComponent();

			_Version.Text = ApplicationInfo.Version;
			_Icon.Source = Images.Get("Logo-256");
		}
	}
}