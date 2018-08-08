using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SLD.Tezos.Client.UI;

namespace SLD.Tezos.Client.Tools
{
	class PropertyMonitor
	{
		List<string> changed = new List<string>();

		public PropertyMonitor(object source)
		{
			if (source is INotifyPropertyChanged npc)
			{
				npc.PropertyChanged += OnPropertyChanged;
			}

			if (source is INotifyCollectionChanged ncc)
			{
				ncc.CollectionChanged += OnCollectionChanged;
			}
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			changed.Add(e.PropertyName);
		}

		internal void AssertOnce(string propertyName)
		{
			Assert.AreEqual(1, changed.Count(n => n == propertyName));
		}

		internal void Clear()
		{
			changed.Clear();
		}
	}
}
