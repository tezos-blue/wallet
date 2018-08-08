using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SLD.Tezos.Client.OS
{
    public interface IBackupIdentity
    {
		string NameKey { get; }
		string DescriptionKey { get; }

		Task<bool?> Backup(string identityID, byte[] backupData);
	}
}
