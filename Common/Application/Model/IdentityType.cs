namespace SLD.Tezos.Client.Model
{
	using Localization;
	using System.Collections.Generic;

	public class IdentityType
	{
		public static IdentityType Standard = new IdentityType
		{
			Stereotype = nameof(Standard),
			UnlockMethod = UnlockMethod.Passphrase,
		};

		public static IdentityType Light = new IdentityType
		{
			Stereotype = nameof(Light),
			UnlockMethod = UnlockMethod.PIN,
		};

		public static IEnumerable<IdentityType> All = new[]
		{
			Standard,
			Light,
		};

		public string Stereotype { get; private set; }

		public string Name => Localizer.Translate($"{Stereotype}Identity");
		public string Description => Localizer.Translate($"About{Stereotype}Identity");

		public UnlockMethod UnlockMethod { get; private set; }

		public static IdentityType Get(string stereotype)
		{
			switch (stereotype)
			{
				case nameof(Light):
					return Light;

				default:
					return Standard;
			}
		}

		public override string ToString() => Stereotype;
	}
}