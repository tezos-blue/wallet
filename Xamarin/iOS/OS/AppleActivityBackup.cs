using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SLD.Tezos.Client.OS
{
	class AppleActivityBackup : IBackupIdentity
	{
		public string NameKey => "Share";

		public string DescriptionKey => throw new NotImplementedException();

		public Task<bool?> Backup(string identityID, byte[] backupData)
		{
			throw new NotImplementedException();

			/*
			 * 
			 * 
			 */
		}
	}
}
