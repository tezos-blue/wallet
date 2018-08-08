using SLD.Tezos.Serialization;

namespace SLD.Tezos.Client.UI
{
	using System;
	using System.Linq;
	using Documents;
	using Security;
	using Cryptography;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using SLD.Tezos.Client.Model;

	public class RestoreIdentityVM : ViewModel
	{
		private IdentityBackupDocument backup;

		public RestoreIdentityVM(Document document)
		{
			this.backup = new IdentityBackupDocument(document);

			PublicKey = new PublicKey(backup.PublicKey);
			Name = backup.Name;
		}

		public PublicKey PublicKey { get; private set; }

		public string IdentityID
			=> PublicKey.Hash;

		public bool IsInCache
			=> Engine.Identities.Any(identity => identity.PublicKey == PublicKey);

		public async Task<Result> Restore(Passphrase passphrase)
		{
			var keyPair = new KeyPair
			(
				PublicKey,
				new PrivateKey(backup.EncryptedPrivateKey)
			);

			if (IsInCache)
			{
				Debug.Assert(passphrase != null);

				if (!keyPair.CanUnlockWith(passphrase))
				{
					return false;
				}
			}

			var identity = await Engine.ImportIdentity(backup.Stereotype, Name, keyPair);

			Main.CurrentIdentity = (IdentityVM)identity;

			return true;
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
					FirePropertyChanged();
				}
			}
		}

		#endregion Name

		#region PassphraseText

		private string _PassphraseText;

		public string PassphraseText
		{
			get
			{
				return _PassphraseText;
			}

			set
			{
				if (_PassphraseText != value)
				{
					_PassphraseText = value;
					FirePropertyChanged();
				}
			}
		}

		#endregion PassphraseText

		public UnlockMethod UnlockMethod
		{
			get
			{
				var identityType = IdentityType.Get(backup.Stereotype);

				return identityType.UnlockMethod;
			}
		}
	}
}