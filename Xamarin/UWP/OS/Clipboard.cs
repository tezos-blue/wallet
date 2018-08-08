using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace SLD.Tezos.Client.OS
{
	public class WindowsClipboard : IClipboard
	{
		public void CopyText(string text)
		{
			var data = new DataPackage();
			data.SetText(text);
			Clipboard.SetContent(data);
		}

		public async Task<string> GetCopiedText()
		{
			var data = Clipboard.GetContent();

			if (data != null && data.Contains(StandardDataFormats.Text))
			{
				return await data.GetTextAsync();
			}

			return null;
		}
	}
}