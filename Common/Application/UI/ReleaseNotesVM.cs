using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace SLD.Tezos.Client.UI
{
    public class ReleaseNotesVM : ViewModel
    {
		static OrderedDictionary notes = new OrderedDictionary 
		{
			{ "0.2.5", "ReleaseNote_0_2_5" }
		};
    }
}
