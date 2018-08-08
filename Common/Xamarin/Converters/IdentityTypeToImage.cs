using System;
using System.Globalization;
using Xamarin.Forms;

namespace SLD.Tezos.Client.Converters
{
	using Model;

	public class IdentityTypeToImage : IValueConverter
	{
		public static ImageSource Convert(IdentityType identity)
		{
			var name = $"{identity.Stereotype}Identity-128";

			return Images.Get(name);
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var identity = (IdentityType)value;

			return Convert(identity);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}