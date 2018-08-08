using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Controls
{
	using Localization;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InfoView : ContentView
	{
		public InfoView()
		{
			InitializeComponent();
			Margin = new Thickness(12);
			VerticalOptions = LayoutOptions.Center;

			OnInfoLevelChanged(InfoLevel);
		}

		#region TextID

		public static BindableProperty TextIDProperty = BindableProperty.Create(
			"TextID", typeof(string),
			typeof(InfoView),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnTextIDChanged,
			defaultValue: null);

		public string TextID
		{
			get
			{
				return (string)GetValue(TextIDProperty);
			}

			set
			{
				SetValue(TextIDProperty, value);
			}
		}

		private static void OnTextIDChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as InfoView).OnTextIDChanged((string)newValue);
		}

		private void OnTextIDChanged(string newTextID)
		{
			_Text.Text = Localizer.Translate(newTextID);
		}

		#endregion TextID

		#region InfoLevel

		public static BindableProperty InfoLevelProperty = BindableProperty.Create(
			"InfoLevel", typeof(InfoLevel),
			typeof(InfoView),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnInfoLevelChanged,
			defaultValue: InfoLevel.Info);

		public InfoLevel InfoLevel
		{
			get
			{
				return (InfoLevel)GetValue(InfoLevelProperty);
			}

			set
			{
				SetValue(InfoLevelProperty, value);
			}
		}

		private static void OnInfoLevelChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as InfoView).OnInfoLevelChanged((InfoLevel)newValue);
		}

		private void OnInfoLevelChanged(InfoLevel newInfoLevel)
		{
			switch (newInfoLevel)
			{
				case InfoLevel.Alert:
					_Line.BackgroundColor = AlertColor;
					break;
				default:
					_Line.BackgroundColor = InfoColor;
					break;
			}
		}

		#endregion InfoLevel

		#region InfoColor

		public static BindableProperty InfoColorProperty = BindableProperty.Create(
			"InfoColor", typeof(Color),
			typeof(InfoView),
			defaultBindingMode: BindingMode.OneWay,
			defaultValue: Color.Blue);

		public Color InfoColor
		{
			get
			{
				return (Color)GetValue(InfoColorProperty);
			}

			set
			{
				SetValue(InfoColorProperty, value);
			}
		}

		#endregion InfoColor

		#region AlertColor

		public static BindableProperty AlertColorProperty = BindableProperty.Create(
			"AlertColor", typeof(Color),
			typeof(InfoView),
			defaultBindingMode: BindingMode.OneWay,
			defaultValue: Color.Red);

		public Color AlertColor
		{
			get
			{
				return (Color)GetValue(AlertColorProperty);
			}

			set
			{
				SetValue(AlertColorProperty, value);
			}
		}

		#endregion AlertColor
	}
}