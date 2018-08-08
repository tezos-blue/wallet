using System.Linq;

namespace SLD.Tezos.Client.UI
{
	using Import;
	using Model;

	public class CreateIdentityVM : ViewModel
	{
		public CreateIdentityVM(IdentityType type)
		{
			IdentityType = type;
		}

		public CreateIdentityVM()
		{
		}

		public IdentityType IdentityType { get; private set; }

		public string Name { get; set; }
		public string Passphrase { get; set; }
		public string RepeatedPassphrase { get; set; }

		public bool CanCreate =>
			!string.IsNullOrWhiteSpace(Name) &&
			!string.IsNullOrWhiteSpace(Passphrase);

		public bool CanLogin
			=> Passphrase == RepeatedPassphrase;

		public async void CreateAndUnlock()
		{
			Identity identity;

			if (IsImport)
			{
				identity = await Engine.ImportIdentity(
					IdentityType.Stereotype,
					Name,
					Importer.CreateKeys(Passphrase));

				if (Importer is FundraiserImport fundraiser)
				{
					// Activation
					if (!string.IsNullOrWhiteSpace(fundraiser.ActivationCode))
					{
						await Engine.ActivateIdentity(identity, fundraiser.ActivationCode, 0); 
					}
				}
			}
			else
			{
				identity = await Engine.AddIdentity(
					IdentityType.Stereotype,
					Name,
					Passphrase);
			}

			identity.Unlock(Passphrase);

			Main.CurrentIdentity = (IdentityVM)identity;
		}

		public void ResetToType(IdentityType type)
		{
			CancelImport();

			IdentityType = type;
		}

		public void CancelImport()
		{
			KeySource = KeyImportSource.Undefined;
			IsKeysDefined = false;

			FirePropertyChanged(nameof(IsKeysDefined));
			FirePropertyChanged(nameof(IsKeysUndefined));
			FirePropertyChanged(nameof(PublicKey));
		}

		#region Key Import

		#region KeySource

		private KeyImportSource _KeySource;

		public KeyImportSource KeySource
		{
			get
			{
				return _KeySource;
			}

			set
			{
				_KeySource = value;

				switch (value)
				{
					case KeyImportSource.Fundraiser:
						Importer = new FundraiserImport();
						break;

					case KeyImportSource.Ed25519:
						Importer = new Ed25519Import();
						break;

					case KeyImportSource.Brain:
						Importer = new BrainImport();
						break;

					default:
						Importer = null;
						break;
				}

				FirePropertyChanged();
				FirePropertyChanged(nameof(IsImport));
				FirePropertyChanged(nameof(IsKeysUndefined));
				FirePropertyChanged(nameof(IsKeysDefined));
			}
		}

		#endregion KeySource

		#region Importer

		private IImportIdentity _Importer;

		public IImportIdentity Importer
		{
			get
			{
				return _Importer;
			}

			private set
			{
				if (_Importer != value)
				{
					_Importer = value;

					FirePropertyChanged();
				}
			}
		}

		#endregion Importer

		public bool IsKeysDefined { get; private set; }

		public bool IsKeysUndefined => !IsKeysDefined;

		public string PublicKey
			=> Importer?.PublicKey?.Hash;

		private bool IsImport
			=> Importer != null;

		public Result Import()
		{
			Importer.FinalizeImport();

			// Check, if the imported identity is already known
			if (Engine.Identities.Any(identity => identity.PublicKey == Importer.PublicKey))
			{
				return Result.Error("IdentityAlreadyKnown");
			}

			IsKeysDefined = true;

			FirePropertyChanged(nameof(IsKeysDefined));
			FirePropertyChanged(nameof(IsKeysUndefined));
			FirePropertyChanged(nameof(PublicKey));

			return true;
		}

		#endregion Key Import
	}
}