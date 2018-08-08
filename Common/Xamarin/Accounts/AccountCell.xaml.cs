using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Accounts
{
	using Model;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountCell : ViewCell
	{
		public AccountCell()
		{
			InitializeComponent();
		}

		#region Account Property

		public static BindableProperty AccountProperty = BindableProperty.Create(
			"Account", typeof(TokenStore),
			typeof(AccountView),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnAccountChanged,
			defaultValue: null);

		public TokenStore Account
		{
			get
			{
				return (TokenStore)GetValue(AccountProperty);
			}

			set
			{
				SetValue(AccountProperty, value);
			}
		}

		private static void OnAccountChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as AccountCell).OnAccountChanged((TokenStore)oldValue, (TokenStore)newValue);
		}

		#endregion Account Property

		private void OnAccountChanged(TokenStore oldAccount, TokenStore newAccount)
		{
			if (oldAccount != null)
			{
				oldAccount.PropertyChanged -= OnAccountPropertyChanged;
			}

			_Layout.BindingContext = newAccount;

			if (newAccount != null)
			{
				newAccount.PropertyChanged += OnAccountPropertyChanged;

				if (newAccount is Identity)
				{
					_Name.FontAttributes = FontAttributes.Italic;
					_Icon.Source = Images.Get("IdentityAccount-128");
				}
				else
				{
					_Name.FontAttributes = FontAttributes.None;

					if (newAccount.State == TokenStoreState.Retired)
					{
						_Icon.Source = Images.Get("Trash-128");
					}
					else
					{
						if (newAccount.IsDelegated)
						{
							_Icon.Source = Images.Get("Delegate-128");
							_Name.Text = (newAccount as Account).DelegateID;
							_Name.FontAttributes = FontAttributes.Italic;
							//_Name.IsVisible = false;
							//_Delegate.IsVisible = true;
						}
						else
						{
							_Icon.Source = Images.Get("ContractAccount-128");
							//_Name.IsVisible = true;
							//_Delegate.IsVisible = false;
						}
					}
				}

				UpdateState();
			}
		}

		private void OnAccountPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "State":
					UpdateState();
					break;

				default:
					break;
			}
		}

		private void UpdateState()
		{
			switch (Account.State)
			{
				case TokenStoreState.Uninitialized:
					_Balance.IsVisible = false;
					_BalanceBusy.IsRunning = false;
					break;

				case TokenStoreState.Creating:
					_Balance.IsVisible = false;
					_BalanceBusy.IsRunning = true;
					break;

				case TokenStoreState.Initializing:
					_Balance.IsVisible = false;
					_BalanceBusy.IsRunning = true;
					break;

				case TokenStoreState.Online:
					_Balance.IsVisible = true;
					_BalanceBusy.IsRunning = false;
					break;

				case TokenStoreState.Offline:
					_Balance.IsVisible = true;
					_BalanceBusy.IsRunning = false;
					break;

				case TokenStoreState.Retired:
					_Icon.Source = Images.Get("Trash-128");
					_Balance.IsVisible = false;
					_BalanceBusy.IsRunning = false;
					break;

				case TokenStoreState.Changing:
					_Balance.IsVisible = true;
					_BalanceBusy.IsRunning = false;
					break;

				default:
					_Layout.BackgroundColor = (Color)App.Current.Resources["Color|Error|Background"];
					break;
			}
		}
	}
}