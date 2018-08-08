using System;

namespace SLD.Tezos.Client.Identities
{
	using Model;
	using Xamarin.Forms;

	public class IdentityTypeSelector : ListView
	{
		public IdentityTypeSelector()
		{
			BackgroundColor = Color.White;

			ItemTemplate = new DataTemplate(typeof(IdentityTypeCell));
			ItemsSource = IdentityType.All;

			HasUnevenRows = true;

			if (Settings.TargetPlatform == Tezos.TargetPlatform.iOS)
			{
				// iOS does not scale right
				RowHeight = 150;
			}

			ItemSelected += (s, e) =>
			{
				if (e.SelectedItem != null)
				{
					IdentityTypeSelected?.Invoke(e.SelectedItem as IdentityType);

					SelectedItem = null;
				}
			};
		}

		public event Action<IdentityType> IdentityTypeSelected;
	}
}