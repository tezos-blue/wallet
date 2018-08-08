using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SLD.Tezos.Client.UI
{
	using Model;
	using Protocol;

	public class AccountVM : ServiceViewModel
	{
		public AccountVM(MainVM parent, TokenStore account) : base(parent)
		{
			Account = account;
			Account.PropertyChanged += OnAccountPropertyChanged;
			Account.EntryAdded += OnEntryAdded;

			if (Account.IsEntriesComplete)
			{
				InitializeEntries();
			}
			else
			{
				Account.CompleteEntries(Engine.Connection);
			}
		}

		public TokenStore Account { get; private set; }

		public ObservableCollection<AccountEntryVM> Entries { get; private set; } = new ObservableCollection<AccountEntryVM>();

		public bool CanSendTransactions
			=> Main.CanSendTransactions && Account.Balance > ApplicationInfo.MinimalAccountBalance;

		public void Delete()
		{
			Engine.DeleteAccount((Account)Account);
		}

		private void OnEntryAdded(AccountEntry entry)
		{
			Entries.Insert(0, new AccountEntryVM(entry));
		}

		protected override void OnServiceStateChanged(ServiceState serviceState)
		{
			FirePropertyChanged(nameof(CanSendTransactions));
		}

		private void OnAccountPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(TokenStore.Entries):
					InitializeEntries();
					break;

				case nameof(TokenStore.Balance):
					FirePropertyChanged(nameof(CanSendTransactions));
					break;

				default:
					break;
			}
		}

		private void InitializeEntries()
		{
			var rawEntries = Account.Entries;

			Entries.Clear();

			foreach (var raw in rawEntries.Reverse())
			{
				Entries.Add(new AccountEntryVM(raw));
			}
		}
	}
}