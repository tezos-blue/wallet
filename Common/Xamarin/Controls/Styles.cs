using System;
using Xamarin.Forms;

namespace SLD.Tezos.Client
{
	internal static class Styles
	{
		public static Color ColorDim => GetColor("Dim");

		public static Color GetColor(string name) => GetResource($"Color|{name}", Color.Blue);

		public static T GetResource<T>(string name, T fallbackValue)
		{
			if (App.Current != null)
			{
				try
				{
					var resource = (T)App.Current.Resources[name];

					if (resource != null)
					{
						return resource;
					}
				}
				catch (Exception e)
				{
					Telemetry.TrackException("Resource lookup failed", e,
						"Name", name
						);
				}
			}

			return fallbackValue;
		}
	}
}