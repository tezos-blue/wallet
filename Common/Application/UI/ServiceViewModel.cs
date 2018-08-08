using System;
using System.Collections.Generic;

namespace SLD.Tezos.Client.UI
{
	using Protocol;

	public class ServiceViewModel : SubViewModel
	{
		private static List<WeakReference<ServiceViewModel>> listeners = new List<WeakReference<ServiceViewModel>>();

		static ServiceViewModel()
		{
			// Register for notifications at the root
			MainVM.Current.Engine.ServiceStateChanged += OnServiceStateChanged;
		}

		public ServiceViewModel(ViewModel parent) : base(parent)
		{
			listeners.Add(new WeakReference<ServiceViewModel>(this));
		}

		protected virtual void OnServiceStateChanged(ServiceState serviceState)
		{
		}

		private static void OnServiceStateChanged(Engine engine)
		{
			// Notify all listeners
			var deprecated = new List<WeakReference<ServiceViewModel>>();

			foreach (var listener in listeners)
			{
				if (listener.TryGetTarget(out ServiceViewModel target))
				{
					target.OnServiceStateChanged(engine.ServiceState);
				}
				else
				{
					deprecated.Add(listener);
				}
			}

			foreach (var listener in deprecated)
			{
				listeners.Remove(listener);
			}
		}
	}
}