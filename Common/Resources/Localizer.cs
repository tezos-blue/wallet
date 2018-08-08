using System.Globalization;

namespace SLD.Tezos.Client.Localization
{
	public static class Localizer
	{
		public static CultureInfo Culture { get; set; } = CultureInfo.CurrentUICulture;

		public static string Translate(string Text, CultureInfo culture = null)
		{
			if (Text == null)
				return "[[Unset]]";

			try
			{
				var translation = TextResources.ResourceManager.GetString(Text, culture ?? Culture);

				return translation ?? $"[{Text}]";
			}
			catch
			{
				return $"[{Text}]!";
			}
		}
	}
}