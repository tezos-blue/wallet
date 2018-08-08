using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SLD.Tezos.Client.Localization
{
	public interface ILocalize
	{
		CultureInfo GetCurrentCulture();

		void SetCulture(CultureInfo culture);
	}
}
