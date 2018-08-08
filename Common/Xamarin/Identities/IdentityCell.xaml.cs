using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Identities
{
	using Service;
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IdentityCell : ViewCell
	{
		public IdentityCell()
		{
			InitializeComponent();
		}

		#region Identity

		public static BindableProperty IdentityProperty = BindableProperty.Create(
			"Identity", typeof(IdentityVM),
			typeof(IdentityCell),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnIdentityChanged,
			defaultValue: null);

		public IdentityVM Identity
		{
			get
			{
				return (IdentityVM)GetValue(IdentityProperty);
			}

			set
			{
				SetValue(IdentityProperty, value);
			}
		}

		private static void OnIdentityChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as IdentityCell).OnIdentityChanged((IdentityVM)oldValue, (IdentityVM)newValue);
		}

		private void OnIdentityChanged(IdentityVM oldIdentity, IdentityVM newIdentity)
		{
			if (oldIdentity != null)
			{
				oldIdentity.PropertyChanged -= OnIdentityPropertyChanged;
			}

			_Layout.BindingContext = newIdentity.Model;

			if (newIdentity != null)
			{
				newIdentity.PropertyChanged += OnIdentityPropertyChanged;

				switch (newIdentity.Model.Stereotype)
				{
					case "Light":
						_Icon.Source = Images.Get("LightIdentity-128");
						break;

					default:
						_Icon.Source = Images.Get("StandardIdentity-128");
						break;
				}
			}
		}

		#endregion Identity

		public void OnClickBackup(object sender, EventArgs args)
		{
			UserInterface.PushPage(new BackupPage { BindingContext = Identity });
		}

		private void OnIdentityPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				default:
					break;
			}
		}
	}
}