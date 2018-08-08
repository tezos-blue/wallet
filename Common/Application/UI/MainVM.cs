using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace SLD.Tezos.Client.UI
{
	using Documents;
	using Model;
	using Protocol;
	using Serialization;

	public delegate void ToSignalServiceState(Engine engine);

	public class MainVM : ViewModel
	{
		private Engine engine;

		public MainVM(Engine engine)
		{
			Current = this;

			this.engine = engine;

			engine.ConnectionStateChanged += OnConnectionStateChanged;

			engine.ServiceStateChanged += OnServiceStateChanged;
			engine.ServiceEventReceived += OnServiceEvent;
			engine.PropertyChanged += OnEnginePropertyChanged;

			if (engine.IsIdentitiesInitialized)
			{
				OnEngineInitialized(true);
			}
			else
			{
				engine.Initialized += OnEngineInitialized;
			}

			CurrentIdentity = (IdentityVM)engine.DefaultIdentity;
		}

		private void OnEngineInitialized(bool success)
		{
			var lastID = ApplicationInfo.LastIdentityID;

			Identity next = null;

			// Check for last stored
			if (lastID != null)
			{
				next = Engine.Identities
					.FirstOrDefault(identity => identity.AccountID == lastID);
			}

			// fallback to default
			if (next == null)
			{
				next = Engine.DefaultIdentity;
			}

			CurrentIdentity = (IdentityVM)next;
		}

		public static MainVM Current { get; private set; }

		public override Engine Engine => engine;

		public bool CanSendTransactions
			=> Engine.IsConnected && ServiceState == ServiceState.Operational;

		public ReleaseNotesVM ReleaseNotes { get; private set; } = new ReleaseNotesVM();

		private void OnEnginePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				//case nameof(Client.Engine.DefaultIdentity):
				//	if (CurrentIdentity == null)
				//	{
				//		_CurrentIdentity = (IdentityVM)Engine.DefaultIdentity;
				//	}
				//	break;

				//case nameof(Client.Engine.IsIdentitiesInitialized):
				//	if (CurrentIdentity == null)
				//	{
				//		var lastID = ApplicationInfo.LastIdentityID;

				//		Identity next = null;

				//		// Check for last stored
				//		if (lastID != null)
				//		{
				//			next = Engine.Identities
				//				.FirstOrDefault(identity => identity.AccountID == lastID);
				//		}

				//		// fallback to default
				//		if (next == null)
				//		{
				//			next = Engine.DefaultIdentity;
				//		}

				//		CurrentIdentity = (IdentityVM)next;
				//	}
				//	break;

				case nameof(Client.Engine.Identities):
					OnIdentitiesChanged();
					break;

				default:
					break;
			}
		}

		#region Identities

		private IdentityVM _CurrentIdentity;

		public bool HasMultipleIdentities
					=> engine.Identities.Count() > 1;

		public bool HasNoIdentity
					=> !engine.Identities.Any();

		public IEnumerable<IdentityVM> Identities
			=> Engine.Identities.Select(identity => (IdentityVM)identity);

		public IdentityVM CurrentIdentity
		{
			get => _CurrentIdentity;

			set
			{
				if (_CurrentIdentity != value)
				{
					_CurrentIdentity = value;

					ApplicationInfo.LastIdentityID = value?.AccountID;

					FirePropertyChanged();
				}
			}
		}

		private void OnIdentitiesChanged()
		{
			FirePropertyChanged(nameof(HasNoIdentity));
			FirePropertyChanged(nameof(HasMultipleIdentities));
		}

		#endregion Identities

		private void OnConnectionStateChanged(Connections.ConnectionState obj)
		{
			FirePropertyChanged(nameof(CanSendTransactions));
		}

		#region Service messages

		public ServiceState ServiceState
					=> engine.ServiceState;

		public ServiceMessageVM ServiceMessage { get; private set; }

		private void OnServiceEvent(Engine engine, ServiceEvent serviceEvent)
		{
			ServiceMessage = new ServiceMessageVM(serviceEvent);

			FirePropertyChanged(nameof(ServiceMessage));
		}

		private void OnServiceStateChanged(Engine engine)
		{
			FirePropertyChanged(nameof(ServiceState));
			FirePropertyChanged(nameof(CanSendTransactions));
		}

		#endregion Service messages

		#region Documents

		public event Action<ViewModel> DisplayRequested;

		#endregion Documents
	}
}