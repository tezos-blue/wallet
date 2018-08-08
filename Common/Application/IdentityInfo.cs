using System;
using System.Collections.Generic;
using System.Text;

namespace SLD.Tezos.Client
{
	class IdentityInfo
	{
		public bool IsBackedUp { get; set; } = false;

		internal void Save(string identityID)
		{
			ApplicationInfo.SaveIdentityInfo(identityID, this);
		}
	}
}
