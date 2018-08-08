using SLD.Tezos.Client.OS;
using System.Threading.Tasks;
using UIKit;

namespace SLD.Tezos.Client.iOS.OS
{
	public class AppleClipboard : IClipboard
	{
		public void CopyText(string text)
		{
			UIPasteboard.General.String = text;
		}

		public Task<string> GetCopiedText()
		{
			return Task.FromResult(UIPasteboard.General.String);
		}
	}
}