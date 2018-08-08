using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SLD.Tezos.Client.OS
{
    public interface IClipboard
    {
		void CopyText(string text);

		Task<string> GetCopiedText();
    }
}
