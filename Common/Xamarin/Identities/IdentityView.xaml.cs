using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Identities
{
	using Model;
	
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IdentityView : ContentView
	{
		public IdentityView()
		{
			InitializeComponent();
		}

		#region Identity

		public static BindableProperty IdentityProperty = BindableProperty.Create(
			"Identity", typeof(Identity),
			typeof(IdentityView),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnIdentityChanged,
			defaultValue: null);

		public Identity Identity
		{
			get
			{
				return (Identity)GetValue(IdentityProperty);
			}

			set
			{
				SetValue(IdentityProperty, value);
			}
		}

		private static void OnIdentityChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as IdentityView).OnIdentityChanged((Identity)oldValue, (Identity)newValue);
		}

		private void OnIdentityChanged(Identity oldIdentity, Identity newIdentity)
		{
			if (oldIdentity != null)
			{
				oldIdentity.PropertyChanged -= OnIdentityPropertyChanged;
			}

			Content.BindingContext = newIdentity;

			if (newIdentity != null)
			{
				newIdentity.PropertyChanged += OnIdentityPropertyChanged;

				switch (newIdentity.Stereotype)
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