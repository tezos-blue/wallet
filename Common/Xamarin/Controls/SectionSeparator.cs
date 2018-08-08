using Xamarin.Forms;

namespace SLD.Tezos.Client.Controls
{
	using Localization;

	public class SectionSeparator : ContentView
	{
		public static BindableProperty ColorProperty = BindableProperty.Create(
			"Color", typeof(Color),
			typeof(SectionSeparator),
			defaultBindingMode: BindingMode.OneWay,
			defaultValue: Styles.ColorDim);

		private BoxView _Line = new BoxView
		{
			HeightRequest = 1,
			Margin = new Thickness(-12, 0),
			Opacity = 0.2,
		};

		private Label _Label = new Label
		{
		};

		public SectionSeparator()
		{
			Margin = new Thickness(0, 24, 0, 12);

			Content = new StackLayout
			{
				Children = {
					_Line,
					_Label,
				}
			};

			_Line.SetBinding(BoxView.ColorProperty, new Binding("Color")
			{
				Source = this,
			});

			_Label.SetBinding(Label.TextColorProperty, new Binding("Color")
			{
				Source = this,
			});
		}

		public Color Color
		{
			get
			{
				return (Color)GetValue(ColorProperty);
			}

			set
			{
				SetValue(ColorProperty, value);
			}
		}

		#region TitleID

		public static BindableProperty TitleIDProperty = BindableProperty.Create(
			"TitleID", typeof(string),
			typeof(SectionSeparator),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnTitleIDChanged,
			defaultValue: "Invalid");

		public string TitleID
		{
			get
			{
				return (string)GetValue(TitleIDProperty);
			}

			set
			{
				SetValue(TitleIDProperty, value);
			}
		}

		private static void OnTitleIDChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var instance = (SectionSeparator)bindable;

			var text = Localizer.Translate((string)newValue);

			instance._Label.Text = text;
		}

		#endregion TitleID

		#region IsLineVisible

		public static BindableProperty IsLineVisibleProperty = BindableProperty.Create(
			"IsLineVisible", typeof(bool),
			typeof(SectionSeparator),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnIsLineVisibleChanged,
			defaultValue: true);


		public bool IsLineVisible
		{
			get
			{
				return (bool)GetValue(IsLineVisibleProperty);
			}

			set
			{
				SetValue(IsLineVisibleProperty, value);
			}
		}

		private static void OnIsLineVisibleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var instance = (SectionSeparator)bindable;

			instance._Line.IsVisible = (bool)newValue;
		}

		#endregion IsLineVisible
	}
}