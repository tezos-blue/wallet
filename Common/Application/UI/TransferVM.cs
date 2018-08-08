using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLD.Tezos.Client.UI
{
	using Model;

	public class TransferVM : TransactionVM
	{
		public TransferVM(ViewModel parent) : base(parent)
		{
		}

		public IEnumerable<TokenStore> AvailableDestinations => Engine
			.GetAvailableDestinations()
			.Where(a => a != SelectedSource)
			.Where(a => a.IsLive);

		public override bool CanCommit =>
			Main.CanSendTransactions &&
			IsSourceSelected &&
			(IsDestinationSelected || IsManualDestinationValid) &&
			IsAmountValid;

		public override decimal MaxAmount
			=> SelectedSource.Balance - ApplicationInfo.MinFeeTransfer;

		public void Commit()
		{
			TokenStore destination;

			if (IsManualDestinationValid)
			{
				destination = new Account(ManualDestinationID, ManualDestinationID);
			}
			else
			{
				destination = SelectedDestination;
			}

			Task.Run(() => Engine.CommitTransfer(SelectedSource, destination, Amount));
		}

		protected override void OnSourceChanged()
		{
			FirePropertyChanged(nameof(AvailableDestinations));
		}

		#region SelectedDestination

		private TokenStore _SelectedDestination;

		public TokenStore SelectedDestination
		{
			get
			{
				return _SelectedDestination;
			}

			set
			{
				if (_SelectedDestination != value)
				{
					_SelectedDestination = value;
					FirePropertyChanged(nameof(SelectedDestination));
					FirePropertyChanged(nameof(CanCommit));
					FirePropertyChanged(nameof(IsDestinationSelected));
					FirePropertyChanged(nameof(IsNoDestinationSelected));
				}
			}
		}

		public bool IsDestinationSelected => SelectedDestination != null;
		public bool IsNoDestinationSelected => SelectedDestination == null;

		#endregion SelectedDestination

		#region ManualDestinationID

		private string _ManualDestinationID;

		public string ManualDestinationID
		{
			get
			{
				return _ManualDestinationID;
			}

			set
			{
				if (_ManualDestinationID != value)
				{
					_ManualDestinationID = value;
					FirePropertyChanged();
					FirePropertyChanged(nameof(CanCommit));
				}
			}
		}

		public bool IsManualDestinationValid => Engine.IsValidAccountID(ManualDestinationID);

		#endregion ManualDestinationID
	}
}