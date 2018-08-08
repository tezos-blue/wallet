namespace SLD.Tezos.Client.UI
{
	using Model;

	public partial class DelegateVM : TransactionVM
	{
		public DelegateVM(ViewModel parent) : base(parent)
		{
			ManagerIdentity = (Identity)Main.CurrentIdentity;
		}

		public override bool CanCommit =>
			Main.CanSendTransactions &&
			IsSourceSelected &&
			IsManualDestinationValid &&
			IsAmountValid;

		public Identity ManagerIdentity { get; set; }

		public override decimal MaxAmount
			=> SelectedSource.Balance - ApplicationInfo.MinFeeOrigination;

		public void Commit()
		{
			Engine.CreateDelegatedAccount(Name, ManagerIdentity, SelectedSource, DelegateID, Amount);
		}

		#region DelegateID

		private string _DelegateID;

		public string DelegateID
		{
			get
			{
				return _DelegateID;
			}

			set
			{
				if (_DelegateID != value)
				{
					_DelegateID = value;
					FirePropertyChanged();
					FirePropertyChanged(nameof(CanCommit));
				}
			}
		}

		public bool IsManualDestinationValid => Engine.IsValidAccountID(DelegateID);

		#endregion DelegateID

		#region Name

		private string _Name;

		public string Name
		{
			get
			{
				return _Name;
			}

			set
			{
				if (_Name != value)
				{
					_Name = value;
					FirePropertyChanged("Name");
				}
			}
		}

		#endregion Name
	}
}