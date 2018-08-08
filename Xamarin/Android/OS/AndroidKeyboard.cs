using Android.Content;
using Android.Views.InputMethods;

namespace SLD.Tezos.Client.OS
{
	internal class AndroidKeyboard : IKeyboard
	{
		private static MainActivity mainActivity;

		public void Hide()
		{
			if (mainActivity.GetSystemService(Context.InputMethodService) is InputMethodManager inputMethodManager)
			{
				var token = mainActivity.CurrentFocus?.WindowToken;
				inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);

				mainActivity.Window.DecorView.ClearFocus();
			}
		}

		internal static void Init(MainActivity mainActivity)
		{
			AndroidKeyboard.mainActivity = mainActivity;
		}
	}
}