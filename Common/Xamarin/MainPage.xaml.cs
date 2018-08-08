using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace SLD.Tezos.Client
{
	using Accounts;
	using Localization;
	using Model;
	using Origination;
	using UI;
	using Service;
	using Identities;

	public partial class MainPage : ContentPage
	{
		#region CurrentIdentity

		public static BindableProperty CurrentIdentityProperty = BindableProperty.Create(
			"CurrentIdentity", typeof(IdentityVM),
			typeof(MainPage),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnCurrentIdentityChanged,
			defaultValue: null);

		public Identity CurrentIdentity
		{
			get => GetValue(CurrentIdentityProperty) as Identity;
			set => SetValue(CurrentIdentityProperty, value);
		}

		private static void OnCurrentIdentityChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as MainPage).SwitchTo((IdentityVM)newValue);
		}

		#endregion CurrentIdentity

		public MainPage()
		{
			InitializeComponent();

			SetBinding(CurrentIdentityProperty, new Binding("CurrentIdentity"));
		}

		public MainVM VM => BindingContext as MainVM;

		internal void Initialize(Engine engine)
		{
			BuildIdentitiesMenu(engine);

			SwitchTo(VM.CurrentIdentity);
		}

		private void BuildIdentitiesMenu(Engine engine)
		{
			if (Settings.TargetPlatform == TargetPlatform.iOS)
			{
				// TODO: iOS does strange things in those context menus. We don't want it.
				return;
			}

			ToolbarItems.Clear();

			if (VM.HasMultipleIdentities)
			{
				foreach (var identity in engine.Identities)
				{
					ToolbarItems.Add(new ToolbarItem(
						name: null,
						icon: null,
						activated: () => VM.CurrentIdentity = (IdentityVM)identity,
						order: ToolbarItemOrder.Secondary
					)
					{
						Text = identity.Name,
					});
				}
			}
		}

		private void SwitchTo(IdentityVM identity)
		{
			Analytics.TrackEvent("Wallet | Main | Switch Identity");

			Title = identity?.Model.Name;
		}

		private void OnClickBackup(object sender, EventArgs args)
		{
			Analytics.TrackEvent("Wallet | Main | Backup");
			Navigation.PushAsync(new BackupPage { BindingContext = VM.CurrentIdentity });
		}

		private void OnAccountSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
			{
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}

			var vm = new AccountVM(VM, e.SelectedItem as TokenStore);

			if (vm.Account.State == TokenStoreState.Retired)
			{
				Analytics.TrackEvent("Wallet | Main | Retired Account");
				Navigation.PushAsync(new AccountNotFoundPage { BindingContext = vm });
			}
			else
			{
				Analytics.TrackEvent("Wallet | Main | Account");
				Navigation.PushAsync(new AccountPage { BindingContext = vm });
			}

			//DisplayAlert("Item Selected", e.SelectedItem.ToString(), "Ok");

			((ListView)sender).SelectedItem = null; //uncomment line if you want to disable the visual selection state.
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			ApplicationCenter.UIReady.SetComplete();
		}
	}
}