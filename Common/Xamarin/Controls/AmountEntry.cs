using Xamarin.Forms;

namespace SLD.Tezos.Client.Controls
{
	public class AmountEntry : ValidatingEntry
	{
		private bool pauseValidation;

		public AmountEntry()
		{
			_Entry.TextChanged += OnTextChanged;
			_Entry.Keyboard = Keyboard.Numeric;
		}

		private bool Validate(decimal amount)
		{
			if (amount > MaxAmount)
			{
				SetInvalid("ExceedsMaxAmount");
				return false;
			}

			if (amount <= 0)
			{
				SetInvalid("NegativeAmount");
				return false;
			}

			SetValid();
			return true;
		}

		#region Amount

		public static BindableProperty AmountProperty = BindableProperty.Create(
			"Amount", typeof(decimal),
			typeof(AmountEntry),
			defaultBindingMode: BindingMode.TwoWay,
			propertyChanged: OnAmountChanged,
			defaultValue: 0.0M);

		public decimal Amount
		{
			get
			{
				return (decimal)GetValue(AmountProperty);
			}

			set
			{
				SetValue(AmountProperty, value);
			}
		}

		private static void OnAmountChanged(BindableObject bindable, object oldValue, object newValue)
			=> (bindable as AmountEntry).OnAmountChanged((decimal)newValue);

		private void OnAmountChanged(decimal amount)
		{
			_Entry.Text = amount.ToString();

			if (!pauseValidation)
			{
				Validate(amount);
			}
		}

		#endregion Amount

		#region MaxAmount

		public static BindableProperty MaxAmountProperty = BindableProperty.Create(
			"MaxAmount", typeof(decimal),
			typeof(AmountEntry),
			defaultBindingMode: BindingMode.OneWay,
			defaultValue: decimal.MaxValue);

		public decimal MaxAmount
		{
			get
			{
				return (decimal)GetValue(MaxAmountProperty);
			}

			set
			{
				SetValue(MaxAmountProperty, value);
			}
		}

		#endregion MaxAmount

		private void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			if (decimal.TryParse(e.NewTextValue, out decimal amount))
			{
				if (Validate(amount))
				{
					pauseValidation = true;
					SetValue(AmountProperty, amount);
					pauseValidation = false;
				}
			}
			else if (string.IsNullOrWhiteSpace(e.NewTextValue))
			{
				SetInvalid("AmountRequired");
			}
			else
			{
				// Display invalid format
				SetInvalid("NotAnAmount");
			}
		}
	}
}