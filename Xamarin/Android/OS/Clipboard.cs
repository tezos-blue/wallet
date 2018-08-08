using Android.Content;
using System.Threading.Tasks;

namespace SLD.Tezos.Client.OS
{
	public class AndroidClipboard : IClipboard
	{
		private static ClipboardManager clipboard;

		public void CopyText(string text)
		{
			var data = ClipData.NewPlainText("tezos-blue", text);

			clipboard.PrimaryClip = data;
		}

		public Task<string> GetCopiedText()
		{
			if (clipboard.HasPrimaryClip)
			{
				var item = clipboard.PrimaryClip.GetItemAt(0);
				var text = item.Text;
				return Task.FromResult(text);
			}

			return Task.FromResult<string>(null);
		}

		internal static void Init(MainActivity mainActivity)
		{
			clipboard = (ClipboardManager)mainActivity.GetSystemService(Context.ClipboardService);
		}
	}
}