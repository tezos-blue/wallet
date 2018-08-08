using System;
using System.Collections.Generic;
using System.Text;

namespace SLD.Tezos.Client.Documents
{
	using Serialization;
	using Model;

	public class IdentityBackupDocument
	{
		private Document document;

		public IdentityBackupDocument(Document document)
		{
			this.document = document;
		}

		public string Name
			=> (string)document["Name"];

		public string Stereotype
			=> (string)document["Stereotype"];

		public byte[] PublicKey
			=> Convert.FromBase64String((string)document["PublicKey"]);

		public byte[] EncryptedPrivateKey
			=> document.Content.BinaryData;

		public static Document Pack(Identity identity)
		{
			var encrypted = identity.BackupData;

			var document = new Document
			{
				DocumentType = DocumentType.IdentityBackup,
				Content = new DataContainer(encrypted),
			};

			document["Name"] = identity.Name;
			document["Stereotype"] = identity.Stereotype;
			document["PublicKey"] = Convert.ToBase64String(identity.PublicKey.Data);

			return document;
		}

	}
}
