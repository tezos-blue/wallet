using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Icons
{
	[ContentProperty("IconName")]
	public class IconExtension : IMarkupExtension
	{
		public string IconName { get; set; }

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			return Images.Get(IconName);
		}
	}
}
