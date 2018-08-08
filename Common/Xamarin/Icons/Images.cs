using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SLD.Tezos.Client
{
    public static class Images
    {
		public static ImageSource Get(string iconname)
		{
			var name = $"SLD.Tezos.Client.Icons.{iconname}.png";
			var assembly = typeof(Images).Assembly;

			return ImageSource.FromResource(name, assembly);
		}
	}
}
