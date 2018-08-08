using SLD.Tezos.Client.OS;
using UIKit;

namespace SLD.Tezos.Client.iOS.OS
{
	internal class AppleKeyboard : IKeyboard
	{
		public void Hide()
		{
			UIApplication.SharedApplication.KeyWindow.EndEditing(true);
		}
	}
}