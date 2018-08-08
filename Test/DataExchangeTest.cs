using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SLD.Tezos.Client
{
	using Documents;
	using OS;
	using Security;
	using Serialization;

	[TestClass]
	public class DataExchangeTest
	{
		[TestMethod]
		public async Task Exchange_IdentityBackup()
		{
			var vault = new SoftwareVault(new TestStorage());

			var identity = await vault.CreateIdentity("Test", "passphrase");

			// Backup
			var document = IdentityBackupDocument.Pack(identity);
			var serialized = document.ToJson();

			// Restore
			var deserialized = Document.FromJson(serialized);
			var received = new IdentityBackupDocument(deserialized);

			var keypair = new KeyPair
			(
				new PublicKey(received.PublicKey),
				new PrivateKey(received.EncryptedPrivateKey)
			);

			var restored = await vault.ImportIdentity(received.Name, keypair, received.Stereotype);

			Assert.AreEqual(identity.Name, restored.Name);
			Assert.AreEqual(identity.Stereotype, restored.Stereotype);
			Assert.AreEqual(identity.PublicKey, restored.PublicKey);

			Assert.IsTrue(vault.Unlock(identity.AccountID, "passphrase"));
		}

		private class TestStorage : IStoreLocal
		{
			private Dictionary<string, MemoryStream> files = new Dictionary<string, MemoryStream>();

			public Task<IEnumerable<Stream>> OpenIdentityFilesAsync()
			{
				var streams = files
					.Select(file => new MemoryStream(file.Value.ToArray()));

				return Task.FromResult(streams.Cast<Stream>());
			}

			public Task<Stream> CreateIdentityFileAsync(string accountID)
			{
				var stream = new MemoryStream();

				files[accountID] = stream;

				return Task.FromResult(stream as Stream);
			}

			public Task DeleteIdentity(string identityID)
			{
				files.Remove(identityID);

				return Task.CompletedTask;
			}

			public Task PurgeAll()
			{
				files.Clear();

				return Task.CompletedTask;
			}
		};
	}
}