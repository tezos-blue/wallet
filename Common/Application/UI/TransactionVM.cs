using System;

namespace SLD.Tezos.Client.UI
{
	using Localization;
	using Model;
	using Protocol;

	public abstract class TransactionVM : ServiceViewModel
	{
		public TransactionVM(ViewModel parent) : base(parent)
		{
		}

		public abstract bool CanCommit { get; }

		public abstract decimal MaxAmount { get; }

		public string AmountHint
			=> Localizer.Translate("Max") + " " + MaxAmount.ToString();

		public void TransferAll()
		{
			Amount = MaxAmount;
		}

		protected override void OnServiceStateChanged(ServiceState serviceState)
		{
			FirePropertyChanged(nameof(CanCommit));
		}

		#region SelectedSource

		private TokenStore _SelectedSource;

		public TokenStore SelectedSource
		{
			get
			{
				return _SelectedSource;
			}

			set
			{
				if (_SelectedSource != value)
				{
					_SelectedSource = value;
					FirePropertyChanged(nameof(SelectedSource));
					FirePropertyChanged(nameof(IsSourceSelected));
					FirePropertyChanged(nameof(IsNoSourceSelected));
				}
			}
		}

		public bool IsSourceSelected => SelectedSource != null;
		public bool IsNoSourceSelected => SelectedSource == null;

		protected virtual void OnSourceChanged()
		{
		}

		#endregion SelectedSource

		#region Amount

		private decimal _Amount;

		public decimal Amount
		{
			get
			{
				return _Amount;
			}

			set
			{
				if (_Amount != value)
				{
					if (value == decimal.MinValue)
					{
						// wrong format
						throw new NotImplementedException();
					}
					else
					{
						_Amount = value;
						FirePropertyChanged();
					}
				}
			}
		}

		#endregion Amount

		#region IsAmountValid

		private bool _IsAmountValid;

		public bool IsAmountValid
		{
			get
			{
				return _IsAmountValid;
			}

			set
			{
				if (_IsAmountValid != value)
				{
					_IsAmountValid = value;
					FirePropertyChanged();
					FirePropertyChanged(nameof(CanCommit));
				}
			}
		}

		#endregion IsAmountValid
	}
}