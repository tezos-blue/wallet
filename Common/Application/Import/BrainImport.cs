namespace SLD.Tezos.Client.Import
{
	using Cryptography;
	using Security;

	public class BrainImport : ClientObject, IImportIdentity
	{
		private string _BrainPrivate;

		private byte[] publicKey, privateKey;

		public string BrainPrivate
		{
			set
			{
				if (_BrainPrivate != value && value != null)
				{
					_BrainPrivate = value;

					FirePropertyChanged(nameof(CanImport));
				}
			}
		}

		public PublicKey PublicKey
		{
			get
			{
				return new PublicKey(publicKey);
			}
		}

		public bool CanImport
			=> IsValidBrain(_BrainPrivate);

		public bool IsValidBrain(string mnemonic)
			=> CryptoServices.IsValidBrain(mnemonic);

		public void FinalizeImport()
		{
			(publicKey, privateKey) = CryptoServices.ImportBrain(_BrainPrivate);

			_BrainPrivate = null;
		}

		public KeyPair CreateKeys(Passphrase passphrase)
		{
			var keypair = new KeyPair
			(
				new PublicKey(publicKey),
				new PrivateKey(privateKey, passphrase)
			);

			privateKey = null;

			return keypair;
		}
	}
}