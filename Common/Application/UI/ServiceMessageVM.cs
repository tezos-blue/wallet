namespace SLD.Tezos.Client.UI
{
	using Localization;
	using Protocol;

	public class ServiceMessageVM : ViewModel
	{
		private ServiceEvent serviceEvent;

		public ServiceMessageVM(ServiceEvent serviceEvent)
		{
			this.serviceEvent = serviceEvent;

			Text = Localizer.Translate($"Service_{serviceEvent.EventID}");
		}

		public string Text { get; private set; }

		public ServiceState ServiceState
			=> serviceEvent.ServiceState;

		public bool ShouldShow
			=> ServiceState != ServiceState.Operational;
	}
}