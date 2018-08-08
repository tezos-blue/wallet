using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Accounts
{
	using Protocol;
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountEntryItemView : ContentView
	{
		public static BindableProperty AccountEntryItemProperty = BindableProperty.Create(
			"AccountEntryItem", typeof(AccountEntryItemVM),
			typeof(AccountEntryItemView),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnAccountEntryItemChanged,
			defaultValue: null);

		public AccountEntryItemView()
		{
			InitializeComponent();

			HorizontalOptions = LayoutOptions.Fill;
		}

		public AccountEntryItemVM AccountEntryItem
		{
			get
			{
				return (AccountEntryItemVM)GetValue(AccountEntryItemProperty);
			}

			set
			{
				SetValue(AccountEntryItemProperty, value);
			}
		}

		private static void OnAccountEntryItemChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as AccountEntryItemView).OnAccountEntryItemChanged((AccountEntryItemVM)oldValue, (AccountEntryItemVM)newValue);
		}

		private void OnAccountEntryItemChanged(AccountEntryItemVM oldAccountEntryItem, AccountEntryItemVM newAccountEntryItem)
		{
			Content.BindingContext = newAccountEntryItem;

			UpdateState();
		}

		private void UpdateState()
		{
		}
	}
}