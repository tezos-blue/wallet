using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Identities
{
	using Converters;
	using Model;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IdentityTypeCell : ViewCell
	{
		//private static IdentityTypeToImage ImageConverter = new IdentityTypeToImage();

		public IdentityTypeCell()
		{
			InitializeComponent();

			//_Image.SetBinding(Image.SourceProperty, new Binding(
			//	"IdentityType",
			//	converter: ImageConverter,
			//	source: this
			//	));

			SetBinding(IdentityTypeProperty, new Binding("."));
		}

		#region IdentityType

		public static BindableProperty IdentityTypeProperty = BindableProperty.Create(
			"IdentityType", typeof(IdentityType),
			typeof(IdentityTypeCell),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnIdentityTypeChanged,
			defaultValue: null);

		public IdentityType IdentityType
		{
			get
			{
				return (IdentityType)GetValue(IdentityTypeProperty);
			}

			set
			{
				SetValue(IdentityTypeProperty, value);
			}
		}

		private static void OnIdentityTypeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as IdentityTypeCell).OnIdentityTypeChanged((IdentityType)oldValue, (IdentityType)newValue);
		}

		private void OnIdentityTypeChanged(IdentityType oldIdentityType, IdentityType newIdentityType)
		{
			_Layout.BindingContext = newIdentityType;

			_Image.Source = IdentityTypeToImage.Convert(newIdentityType);
		}

		#endregion IdentityType
	}
}