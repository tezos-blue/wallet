using System;
using System.Globalization;
using Xamarin.Forms;

namespace SLD.Tezos.Client.Converters
{
	public class UtcToLocalTime : IValueConverter
	{
		public static DateTime Convert(DateTime utc)
		{
			return utc;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Convert((DateTime)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}