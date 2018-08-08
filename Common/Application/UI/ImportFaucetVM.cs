namespace SLD.Tezos.Client.UI
{
	using Cryptography;
	using Documents;
	using Model;
	using Security;

	public class ImportFaucetVM : ViewModel
	{
		public SyncEvent WhenComplete = new SyncEvent();
		private FaucetDocument faucet;

		public ImportFaucetVM(FaucetDocument faucet)
		{
			this.faucet = faucet;
		}

		public async void Import()
		{
			//BIP 39
			var sentence = string.Join(" ", faucet.mnemonic);

			(byte[] publicKey, byte[] privateKey) = CryptoServices.ImportBIP39(sentence, faucet.email, faucet.password);

			var keypair = new KeyPair
			(
				new PublicKey(publicKey),
				new PrivateKey(privateKey, PIN)
			);

			// Import Identity
			var identity = Engine.FindIdentity(keypair.PublicID);

			if (identity == null)
			{
				identity = await Engine.ImportIdentity(
					IdentityType.Light.Stereotype,
					Name,
					keypair);
			}

			identity.Unlock(PIN);

			Main.CurrentIdentity = (IdentityVM)identity;

			WhenComplete.SetComplete();

			// Activate
			await Engine.ActivateIdentity(identity, faucet.secret, faucet.ModelAmount);
		}

		public void Cancel()
			=> WhenComplete.Cancel();

		#region Display Properties

		private string _PIN;

		private string _RepeatPIN;

		public string IdentityID
			=> faucet.pkh;

		public decimal Amount
			=> faucet.ModelAmount;

		public string Name { get; set; }

		public string PIN
		{
			get => _PIN;
			set
			{
				_PIN = value;
				FirePropertyChanged(nameof(CanImport));
			}
		}

		public string RepeatPIN
		{
			get => _RepeatPIN;
			set
			{
				_RepeatPIN = value;
				FirePropertyChanged(nameof(CanImport));
			}
		}

		public bool CanImport
		{
			get
			{
				if (string.IsNullOrWhiteSpace(PIN)) return false;
				if (string.IsNullOrWhiteSpace(RepeatPIN)) return false;

				var pin = PIN.Trim();

				if (pin.Length != 4) return false;

				return pin == RepeatPIN.Trim();
			}
		}

		#endregion Display Properties
	}
}