using System;
using System.Globalization;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Localization
{
	[ContentProperty("Text")]
	public class TranslateExtension : IMarkupExtension
	{
		private ResourceManager Resources => TextResources.ResourceManager;

		private readonly CultureInfo culture;

		public TranslateExtension()
		{
			if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
			{
				culture = DependencyService.Get<ILocalize>().GetCurrentCulture();
			}
		}

		public string Text { get; set; }

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			return Localizer.Translate(Text, culture);
		}
	}
}