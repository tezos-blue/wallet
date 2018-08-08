using System.Threading.Tasks;

namespace SLD.Tezos.Client.UI
{
	using Documents;
	using Model;
	using OS;
	using System.Collections.Generic;

	public class IdentityVM : ViewModel
	{
		private static Dictionary<string, IdentityVM> pool = new Dictionary<string, IdentityVM>();

		public Identity Model { get; set; }

		private IdentityInfo info;

		private IdentityVM(Identity identity)
		{
			this.Model = identity;
			this.info = ApplicationInfo.LoadIdentityInfo(identity.AccountID);
		}

		public string AccountID => Model.AccountID;

		public bool HasName
			=> AccountID != Model.Name;

		#region Conversion

		public static explicit operator Identity(IdentityVM vm)
			=> vm?.Model;

		public static explicit operator IdentityVM(Identity identity)
		{
			if (identity == null)
			{
				return null;
			}

			if (!pool.TryGetValue(identity.AccountID, out IdentityVM vm))
			{
				vm = new IdentityVM(identity);

				pool.Add(identity.AccountID, vm);
			}

			return vm;
		}

		#endregion Conversion

		#region Backup

		public bool IsBackedUp
		{
			get => info.IsBackedUp;

			set
			{
				info.IsBackedUp = value;
				info.Save(AccountID);

				FirePropertyChanged(nameof(IsBackedUp));
				FirePropertyChanged(nameof(NeedsBackup));
			}
		}

		public bool NeedsBackup => !IsBackedUp;

		public async Task Backup(IBackupIdentity backupProvider)
		{
			var document = IdentityBackupDocument.Pack(Model);

			bool? wasSent = await backupProvider.Backup(Model.AccountID, document.ToArray());

			if (wasSent.HasValue)
			{
				IsBackedUp = wasSent.Value;
			}
		}

		#endregion Backup
	}
}