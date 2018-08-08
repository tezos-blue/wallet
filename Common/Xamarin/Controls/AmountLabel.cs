using System;

using Xamarin.Forms;

namespace SLD.Tezos.Client.Controls
{
	public class AmountLabel : Label
	{
		public static BindableProperty AmountProperty = BindableProperty.Create(
			"Amount", typeof(decimal),
			typeof(AmountLabel),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnAmountChanged,
			defaultValue: -1M);

		//private Label _Label = new Label
		//{
		//	WidthRequest = 150,
		//	HorizontalTextAlignment = TextAlignment.End,
		//};

		public AmountLabel()
		{
			BackgroundColor = Color.Transparent;
			//WidthRequest = 150;
			HorizontalTextAlignment = TextAlignment.End;
			LineBreakMode = LineBreakMode.NoWrap;
			//Content = new StackLayout
			//{
			//	Children = {
			//		_Label,
			//	}
			//};
		}

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
		{
			(bindable as AmountLabel).OnAmountChanged((decimal)newValue);
		}

		private void OnAmountChanged(decimal newValue)
		{
			Text = newValue.ToString("N2");
			//HorizontalTextAlignment = TextAlignment.End;

			if (Amount < 0)
			{
				TextColor = Styles.GetColor("Amount|Negative");
			}
		}
	}
}