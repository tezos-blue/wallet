namespace SLD.Tezos.Client.Import
{
	using Cryptography;
	using Security;

	public class Ed25519Import : ClientObject, IImportIdentity
	{
		private string _Ed25519Private;

		public string Ed25519Private
		{
			set
			{
				if (_Ed25519Private != value)
				{
					_Ed25519Private = value;

					FirePropertyChanged(nameof(CanImport));
				}
			}
		}

		public PublicKey PublicKey => new PublicKey(publicKey);

		public bool CanImport
			=> IsValidEd25519(_Ed25519Private);

		public bool IsValidEd25519(string privateKey)
			=> CryptoServices.IsValidEd25519(privateKey);

		byte[] publicKey, privateKey;

		public void FinalizeImport()
		{
			(publicKey, privateKey) = CryptoServices.ImportEd25519(_Ed25519Private);

			_Ed25519Private = null;
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