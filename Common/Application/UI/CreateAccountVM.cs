namespace SLD.Tezos.Client.UI
{
	using Model;
	using Protocol;

	public partial class CreateAccountVM : TransactionVM
	{
		public CreateAccountVM(ViewModel parent) : base(parent)
		{
			ManagerIdentity = (Identity)Main.CurrentIdentity;
		}

		public Identity ManagerIdentity { get; set; }

		public override bool CanCommit
		{
			get
			{
				// Only while service is up
				if (Engine.IsConnected && Engine.ServiceState == ServiceState.Operational)
				{
					// Source selected
					if (SelectedSource == null)
						return false;

					// Funds and Transfer amount
					return IsAmountValid;
				}

				return false;
			}
		}

		public override decimal MaxAmount
			=> SelectedSource.Balance - ApplicationInfo.MinFeeOrigination
			+ (SelectedSource.IsIdentityAccount ? ToDo.PatchOriginateFromIdentity : 0);

		public void Create()
		{
			Engine.CreateAccount(Name, ManagerIdentity, SelectedSource, Amount);
		}

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