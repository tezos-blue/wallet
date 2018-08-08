using System.ComponentModel;

namespace SLD.Tezos.Client.Import
{
	using Cryptography;
	using Security;

	public interface IImportIdentity : INotifyPropertyChanged
	{
		bool CanImport { get; }

		PublicKey PublicKey { get; }

		void FinalizeImport();

		KeyPair CreateKeys(Passphrase passphrase);
	}
}