using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Resources;

[assembly: NeutralResourcesLanguage("en")]

namespace SLD.Tezos.Client
{
	using Serialization;

	public static class ApplicationInfo
	{
		public const string ProductName = "tezos.blue";
		public const string Version = "0.3.1";

		public const string Company = "Schloss Lautereck Design";
		public const string Copyright = "© 2018";

		public const string UWPStoreLink = "https://www.microsoft.com/de-de/store/p/tezosblue/9nkzfw5334gt";


		#region Settings

		public static string LastIdentityID
		{
			get => UserSettings.GetValueOrDefault(nameof(LastIdentityID), null);
			set => UserSettings.AddOrUpdateValue(nameof(LastIdentityID), value);
		}

		public static string LastApplicationVersion
		{
			get => UserSettings.GetValueOrDefault(nameof(LastApplicationVersion), null);
			set => UserSettings.AddOrUpdateValue(nameof(LastApplicationVersion), value);
		}

		private static ISettings UserSettings => CrossSettings.Current;

		#endregion Settings

		#region Constants

		public const decimal MinimalAccountBalance = 1;

		public static TimeSpan GetUnlockDelay(int failedUnlocks)
			=> TimeSpan.FromSeconds(3);

		public static ServiceInfo ServiceInfo;

		public static decimal FeeOriginationBaker
			=> ServiceInfo["Origination"]["Baker"].Fee;

		public static decimal FeeOriginationStorage
			=> ServiceInfo["Origination"]["Storage"].Fee;

		public static decimal FeeOriginationService
			=> ServiceInfo["Origination"]["Service"].Fee;

		public static decimal FeeTransferBaker
			=> ServiceInfo["Transfer"]["Baker"].Fee;

		public static decimal FeeTransferService
			=> ServiceInfo["Transfer"]["Service"].Fee;

		public static decimal MinFeeTransfer
			=> FeeTransferBaker + FeeTransferService;

		public static decimal MinFeeOrigination
			=> FeeOriginationBaker + FeeOriginationStorage + FeeOriginationService;

		#endregion Constants

		#region License Agreement

		public static DateTime? AgreementSignedDate
		{
			get
			{
				var value = UserSettings.GetValueOrDefault(nameof(AgreementSignedDate), null);

				if (value != null)
				{
					return DateTime.Parse(value);
				}

				return null;
			}
		}

		public static bool IsLicensed
			=> AgreementSignedDate.HasValue;

		public static void SignAgreement()
		{
			UserSettings.AddOrUpdateValue(nameof(AgreementSignedDate), DateTime.UtcNow.ToSafeString());
		}

		#endregion License Agreement

		#region Identity Settings

		internal static IdentityInfo LoadIdentityInfo(string identityID)
		{
			string json = UserSettings.GetValueOrDefault(identityID, null);

			if (json != null)
			{
				return json.ToModelObject<IdentityInfo>();
			}

			return new IdentityInfo();
		}

		internal static void SaveIdentityInfo(string identityID, IdentityInfo info)
		{
			string json = info.ToJson();

			UserSettings.AddOrUpdateValue(identityID, json);
		}

		#endregion Identity Settings
	}
}