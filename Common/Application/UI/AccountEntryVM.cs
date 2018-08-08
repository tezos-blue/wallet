using System.Linq;

namespace SLD.Tezos.Client.UI
{
	using Localization;
	using Protocol;

	public class AccountEntryVM : ViewModel
	{
		public AccountEntryVM(AccountEntry entry)
		{
			this.Entry = entry;

			Items = entry.Items
				.Select(i => new AccountEntryItemVM(i))
				.ToArray();
		}

		public AccountEntry Entry { get; private set; }

		public string Time => Entry.TimeGMT
			.ToLocalTime()
			.ToString("g", Localizer.Culture);

		public bool HasFee => Entry.Fees != 0;

		public decimal NetworkFeePaid => -Entry.Fees;

		public AccountEntryItemVM[] Items { get; private set; }
	}
}