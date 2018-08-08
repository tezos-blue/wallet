using System.Collections.Generic;
using System.Linq;

namespace SLD.Tezos.Client.Import
{
	using Cryptography;
	using Localization;
	using Security;

	public class FundraiserImport : ClientObject, IImportIdentity
	{
		private List<string> words = new List<string>();

		public string Sentence
			=> string.Join(" ", words);

		public bool IsSentenceComplete
			=> words.Count == 15;

		public bool IsSentenceInvalid { get; private set; }
		public bool IsSentenceValid => !IsSentenceInvalid;

		public string WordTip
			=> $"{Localizer.Translate("Word")} {words.Count + 1}";

		public bool CanImport
			=> IsSentenceComplete
			&& IsCredentialsFilled;
			//&& !string.IsNullOrWhiteSpace(_ActivationCode);

		public bool IsCredentialsFilled
			=> !string.IsNullOrWhiteSpace(_EMail)
			&& !string.IsNullOrEmpty(_Passphrase);

		public PublicKey PublicKey => new PublicKey(publicKey);

		string[] WordPool => CryptoServices.BIP39EnglishWords;

		#region CurrentWord

		private string _CurrentWord;

		public string CurrentWord
		{
			get
			{
				return _CurrentWord;
			}

			set
			{
				if (_CurrentWord != value)
				{
					_CurrentWord = value;

					Guess();

					FirePropertyChanged();
				}
			}
		}

		#endregion CurrentWord

		#region BestGuess

		private string _BestGuess;

		public string BestGuess
		{
			get
			{
				return _BestGuess;
			}

			set
			{
				if (_BestGuess != value)
				{
					_BestGuess = value;
					FirePropertyChanged();
					FirePropertyChanged(nameof(CanGuess));
				}
			}
		}

		#endregion BestGuess

		#region CanGuess

		private bool _CanGuess;

		public bool CanGuess
		{
			get
			{
				return _CanGuess;
			}

			set
			{
				if (_CanGuess != value)
				{
					_CanGuess = value;
					FirePropertyChanged();
				}
			}
		}

		#endregion CanGuess

		public void SelectGuess()
		{
			words.Add(BestGuess);

			FirePropertyChanged(nameof(Sentence));

			if (IsSentenceComplete)
			{
				// Check BIP39 validity
				if (CryptoServices.IsValidBIP39(Sentence))
				{
					FirePropertyChanged(nameof(IsSentenceComplete));
				}
				else
				{
					IsSentenceInvalid = true;
					FirePropertyChanged(nameof(IsSentenceInvalid));
					FirePropertyChanged(nameof(IsSentenceValid));
				}
			}
			else
			{
				// Clear for next word
				CurrentWord = string.Empty;
				FirePropertyChanged(nameof(WordTip));
			}
		}

		byte[] publicKey, privateKey;

		public void FinalizeImport()
		{
			(publicKey, privateKey) = DeriveKeys();

			Reset();
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

		public void Reset()
		{
			words.Clear();
			CurrentWord = string.Empty;
			Passphrase = null;
			EMail = null;
			//ActivationCode = null; Will be needed
			IsSentenceInvalid = false;

			FirePropertyChanged(nameof(Sentence));
			FirePropertyChanged(nameof(IsSentenceComplete));
			FirePropertyChanged(nameof(IsSentenceInvalid));
			FirePropertyChanged(nameof(IsSentenceValid));
			FirePropertyChanged(nameof(WordTip));
			FirePropertyChanged(nameof(CanImport));
			FirePropertyChanged(nameof(PublicKey));
		}

		private (byte[], byte[]) DeriveKeys()
			=> CryptoServices.ImportBIP39(Sentence, _EMail.Trim(), _Passphrase);

		private void Guess()
		{
			if (!string.IsNullOrWhiteSpace(CurrentWord))
			{
				var candidates = WordPool.Where(word => word.StartsWith(CurrentWord.Trim().ToLower()));

				CanGuess = candidates.Any();

				BestGuess = CanGuess ? candidates.First() : string.Empty;
			}
		}

		#region EMail

		private string _EMail;

		public string EMail
		{
			set
			{
				if (_EMail != value)
				{
					_EMail = value;
					FirePropertyChanged(nameof(CanImport));
					FirePropertyChanged(nameof(CurrentHash));
				}
			}
		}

		#endregion EMail

		#region Passphrase

		private string _Passphrase;

		public string Passphrase
		{
			set
			{
				if (_Passphrase != value)
				{
					_Passphrase = value;
					FirePropertyChanged(nameof(CanImport));
					FirePropertyChanged(nameof(CurrentHash));
				}
			}
		}

		#endregion Passphrase

		#region ActivationCode

		private string _ActivationCode;

		public string ActivationCode
		{
			get => _ActivationCode;

			set
			{
				if (_ActivationCode != value)
				{
					_ActivationCode = value;
					//FirePropertyChanged(nameof(CanImport));
				}
			}
		}

		#endregion ActivationCode

		public string CurrentHash
		{
			get
			{
				if (!IsCredentialsFilled)
				{
					return null;
				}

				(var publicKey, var privateKey) = DeriveKeys();

				var key = new PublicKey(publicKey);

				return key.Hash;
			}
		}
	}
}