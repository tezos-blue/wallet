using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SLD.Tezos.Client.Controls
{
	using Cryptography;
	using Localization;
	using Model;

	public class CredentialEntry : Grid
	{
		private Entry entry;
		private Button button;

		private int failedUnlocks;

		public CredentialEntry()
		{
			HorizontalOptions = LayoutOptions.FillAndExpand;

			ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
			ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

			entry = new Entry
			{
				IsPassword = true,
			};

			entry.TextChanged += (s, e) => OnCredentialChanged(e.NewTextValue);

			button = new Button
			{
				IsEnabled = false,
			};

			SetColumn(button, 1);

			button.Clicked += (s, e) => Commit(NeedsCredentials ? entry.Text : null);

			Children.Add(entry);
			Children.Add(button);
		}

		public event Action<Passphrase> Committed;

		private bool UsePIN
			=> UnlockMethod == UnlockMethod.PIN;

		public void InitializeFocus()
		{
			if (UsePIN && NeedsCredentials)
			{
				entry.Focus();
			}
		}

		public async void RetryAfterFail()
		{
			failedUnlocks++;

			entry.Text = string.Empty;
			entry.Placeholder = Localizer.Translate(UsePIN ? "InvalidPINTryAgain" : "InvalidPassphraseTryAgain");
			entry.IsEnabled = false;

			await Task.Delay(ApplicationInfo.GetUnlockDelay(failedUnlocks));

			entry.IsEnabled = true;
			entry.Focus();
		}

		private void Initialize()
		{
			button.Text = Localizer.Translate(ButtonTextID);
			button.IsEnabled = !NeedsCredentials;
			entry.IsVisible = NeedsCredentials;

			if (UsePIN)
			{
				entry.Placeholder = Localizer.Translate(PINTextID);
				entry.HorizontalTextAlignment = TextAlignment.Center;
				entry.Keyboard = Keyboard.Telephone;
				button.IsVisible = !NeedsCredentials;
			}
			else
			{
				entry.Placeholder = Localizer.Translate("Passphrase");
				entry.HorizontalTextAlignment = TextAlignment.Start;
				entry.Keyboard = Keyboard.Text;
				button.IsVisible = true;
			}

			entry.HorizontalOptions = LayoutOptions.FillAndExpand;
			entry.IsPassword = true;
		}

		private void Commit(string credential)
			=> Committed?.Invoke(credential);

		private void OnCredentialChanged(string credential)
		{
			if (UsePIN)
			{
				if (credential.Length == Settings.PINLength)
				{
					App.Keyboard.Hide();
					Commit(credential);
				}
			}
			else
			{
				button.IsEnabled = !string.IsNullOrEmpty(credential);
			}
		}

		#region UnlockMethod

		public static BindableProperty UnlockMethodProperty = BindableProperty.Create(
			"UnlockMethod", typeof(UnlockMethod),
			typeof(CredentialEntry),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnUnlockMethodChanged,
			defaultValue: UnlockMethod.Passphrase);

		public UnlockMethod UnlockMethod
		{
			get
			{
				return (UnlockMethod)GetValue(UnlockMethodProperty);
			}

			set
			{
				SetValue(UnlockMethodProperty, value);
			}
		}

		private static void OnUnlockMethodChanged(BindableObject bindable, object oldValue, object newValue)
			=> (bindable as CredentialEntry).OnUnlockMethodChanged((UnlockMethod)newValue);

		private void OnUnlockMethodChanged(UnlockMethod unlockMethod)
		{
			Initialize();
		}

		#endregion UnlockMethod

		#region NeedsCredentials

		public static BindableProperty NeedsCredentialsProperty = BindableProperty.Create(
			"NeedsCredentials", typeof(bool),
			typeof(CredentialEntry),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnNeedsCredentialsChanged,
			defaultValue: true);

		public bool NeedsCredentials
		{
			get
			{
				return (bool)GetValue(NeedsCredentialsProperty);
			}

			set
			{
				SetValue(NeedsCredentialsProperty, value);
			}
		}

		private static void OnNeedsCredentialsChanged(BindableObject bindable, object oldValue, object newValue)
			=> (bindable as CredentialEntry).OnNeedsCredentialsChanged((bool)newValue);

		private void OnNeedsCredentialsChanged(bool key)
		{
			Initialize();
		}

		#endregion NeedsCredentials

		#region ButtonTextID

		public static BindableProperty ButtonTextIDProperty = BindableProperty.Create(
			"ButtonTextID", typeof(string),
			typeof(CredentialEntry),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnButtonTextIDChanged,
			defaultValue: null);

		public string ButtonTextID
		{
			get
			{
				return (string)GetValue(ButtonTextIDProperty);
			}

			set
			{
				SetValue(ButtonTextIDProperty, value);
			}
		}

		private static void OnButtonTextIDChanged(BindableObject bindable, object oldValue, object newValue)
			=> (bindable as CredentialEntry).OnButtonTextIDChanged((string)newValue);

		private void OnButtonTextIDChanged(string key)
		{
			Initialize();
		}

		#endregion ButtonTextID

		#region PINTextID

		public static BindableProperty PINTextIDProperty = BindableProperty.Create(
			"PINTextID", typeof(string),
			typeof(CredentialEntry),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnPINTextIDChanged,
			defaultValue: null);

		public string PINTextID
		{
			get
			{
				return (string)GetValue(PINTextIDProperty);
			}

			set
			{
				SetValue(PINTextIDProperty, value);
			}
		}

		private static void OnPINTextIDChanged(BindableObject bindable, object oldValue, object newValue)
			=> (bindable as CredentialEntry).OnPINTextIDChanged((string)newValue);

		private void OnPINTextIDChanged(string key)
		{
			Initialize();
		}

		#endregion PINTextID
	}
}