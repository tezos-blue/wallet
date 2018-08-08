using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SLD.Tezos.Client.Documents
{
	using Serialization;

	public class FaucetDocument
    {
		public string[] mnemonic;

		public string secret;
		public decimal amount;
		public string pkh;
		public string password;
		public string email;

		public decimal ModelAmount
			=> amount / 1000000;

		public static FaucetDocument FromJson(string json)
		{
			var document = json.ToModelObject<FaucetDocument>();

			if (document.secret == null)
			{
				throw new InvalidDataContractException("Not a Document");
			}

			return document;
		}
			
    }
}
