namespace SLD.Tezos.Client.UI
{
	using Localization;
	using Protocol;

	public class AccountEntryItemVM : ViewModel
	{
		public AccountEntryItemVM(AccountEntryItem item)
		{
			this.Item = item;
		}

		public AccountEntryItem Item { get; private set; }

		public bool IsOutbound => Item.Amount < 0;

		public string Text
		{
			get
			{
				switch (Item.Kind)
				{
					case AccountEntryItemKind.Origination:
						if (IsOutbound)
						{
							return $"{Localizer.Translate("FundedAccount")}: {Item.ContraAccountID}";
						}
						else
						{
							return $"{Localizer.Translate("FundedBy")}: {Item.ContraAccountID}";
						}

					case AccountEntryItemKind.Transfer:
						if (IsOutbound)
						{
							return $"{Localizer.Translate("TransferredTo")}: {Item.ContraAccountID}";
						}
						else
						{
							return $"{Localizer.Translate("ReceivedBy")}: {Item.ContraAccountID}";
						}

					case AccountEntryItemKind.Internal:
						return Localizer.Translate("InternalTransfer");

					case AccountEntryItemKind.Activation:
						return Localizer.Translate("Activation");

					case AccountEntryItemKind.Invalid:
					default:
						return Localizer.Translate("Invalid");
				}
			}
		}
	}
}