using System;

using Xamarin.Forms;

namespace SLD.Tezos.Client.Controls
{
	using Localization;

	public class ValidatingEntry : ContentView
	{
		protected Entry _Entry = new Entry();
		protected Label _Message = new Label
		{
			TextColor = Color.Red,
			FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
		};

		public ValidatingEntry()
		{
			Content = new StackLayout
			{
				Children = {
					_Entry,
					_Message,
				}
			};
		}

		public void SetInvalid(string messageID)
		{
			_Message.Text = Localizer.Translate(messageID);
			IsValid = false;
		}

		public void SetValid()
		{
			_Message.Text = null;
			IsValid = true;
		}

		#region IsValid

		public static BindableProperty IsValidProperty = BindableProperty.Create(
			"IsValid", typeof(bool),
			typeof(AmountEntry),
			defaultBindingMode: BindingMode.OneWayToSource,
			defaultValue: false);

		public bool IsValid
		{
			get
			{
				return (bool)GetValue(IsValidProperty);
			}

			private set
			{
				SetValue(IsValidProperty, value);
			}
		}

		#endregion IsValid

		#region Placeholder

		public static BindableProperty PlaceholderProperty = BindableProperty.Create(
			"Placeholder", typeof(string),
			typeof(AmountEntry),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnPlaceholderChanged,
			defaultValue: null);

		private static void OnPlaceholderChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as AmountEntry)._Entry.Placeholder = newValue as string;
		}

		public string Placeholder
		{
			get
			{
				return (string)GetValue(PlaceholderProperty);
			}

			private set
			{
				SetValue(PlaceholderProperty, value);
			}
		}

		#endregion Placeholder

	}
}