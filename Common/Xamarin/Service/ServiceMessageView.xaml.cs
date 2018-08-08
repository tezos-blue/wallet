using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLD.Tezos.Client.Service
{
	using Protocol;
	using System.Threading.Tasks;
	using UI;

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ServiceMessageView : ContentView
	{
		public ServiceMessageView()
		{
			IsVisible = false;
			InitializeComponent();

			SetBinding(MessageProperty, new Binding(
				"ServiceMessage",
				source: MainVM.Current
				));
		}

		private async void OnMessageChanged()
		{
			if (Message != null)
			{
				// Show
				_Message.Text = Message.Text;
				SetState(Message.ServiceState);

				if (Message.ShouldShow)
				{
					IsVisible = true;
				}
				else
				{
					// Override by ShowAll
					if (ShowAll)
					{
						IsVisible = true;

						// remove after timeout
						await Task.Delay(Settings.TimeToShowInfoMessages);
					}

					IsVisible = false;
				}
			}
			else
			{
				// no message
				IsVisible = false;
			}
		}

		private void SetState(ServiceState serviceState)
		{
			try
			{
				switch (serviceState)
				{
					case ServiceState.Unknown:
						_Layout.BackgroundColor = Styles.GetColor("Pending|Background");
						break;

					case ServiceState.Operational:
						_Layout.BackgroundColor = Styles.GetColor("Blue|Background");
						break;

					case ServiceState.Limited:
					case ServiceState.Down:
					default:
						_Layout.BackgroundColor = Styles.GetColor("Error|Background");
						break;
				}
			}
			catch (Exception)
			{
				// TODO Crash when launching with ServiceMessage, Colors not known...
			}
		}

		#region Message

		public static BindableProperty MessageProperty = BindableProperty.Create(
			"Message", typeof(ServiceMessageVM),
			typeof(ServiceMessageView),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnMessageChanged,
			defaultValue: null);

		public ServiceMessageVM Message
		{
			get => GetValue(MessageProperty) as ServiceMessageVM;
			set => SetValue(MessageProperty, value);
		}

		private static void OnMessageChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as ServiceMessageView).OnMessageChanged();
		}

		#endregion Message

		#region ShowAll

		public static BindableProperty ShowAllProperty = BindableProperty.Create(
			"ShowAll", typeof(bool),
			typeof(ServiceMessageView),
			defaultBindingMode: BindingMode.TwoWay,
			propertyChanged: OnShowAllChanged,
			defaultValue: false);

		public bool ShowAll
		{
			get => (bool)GetValue(ShowAllProperty);
			set => SetValue(ShowAllProperty, value);
		}

		private static void OnShowAllChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as ServiceMessageView).OnMessageChanged();
		}

		#endregion ShowAll
	}
}