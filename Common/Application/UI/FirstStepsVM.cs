namespace SLD.Tezos.Client.UI
{
	using Model;

	public class FirstStepsVM : ViewModel
	{
		public string IdentityName { get; set; }

		public string IdentityPassphrase { get; set; } = TempPassphrase;

		public string RepeatPassphrase { get; set; }

		public async void CheckAndSignIn()
		{
			await Engine.AddIdentity(IdentityType.Standard.Stereotype, IdentityName, IdentityPassphrase);
		}
	}
}