using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Accounts
{
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountEntryView : ContentView
	{
		public static BindableProperty AccountEntryProperty = BindableProperty.Create(
			"AccountEntry", typeof(AccountEntryVM),
			typeof(AccountEntryView),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnAccountEntryChanged,
			defaultValue: null);

		public AccountEntryView()
		{
			InitializeComponent();

			HorizontalOptions = LayoutOptions.FillAndExpand;
		}

		public AccountEntryVM AccountEntry
		{
			get
			{
				return (AccountEntryVM)GetValue(AccountEntryProperty);
			}

			set
			{
				SetValue(AccountEntryProperty, value);
			}
		}

		internal void Shrink()
		{
			_Layout.Children.Remove(_Balance);
		}

		private static void OnAccountEntryChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as AccountEntryView).OnAccountEntryChanged((AccountEntryVM)oldValue, (AccountEntryVM)newValue);
		}

		private void OnAccountEntryChanged(AccountEntryVM oldAccountEntry, AccountEntryVM newAccountEntry)
		{
			Content.BindingContext = newAccountEntry;

			if (newAccountEntry != null)
			{
				_Items.Children.Clear();

				foreach (var item in newAccountEntry.Items)
				{
					_Items.Children.Add(new AccountEntryItemView
					{
						AccountEntryItem = item,
					});
				}
			}
		}
	}
}