using System;
using System.Linq;
using System.Collections.Specialized;
using Xamarin.Forms;
using System.Collections.Generic;

namespace SLD.Tezos.Client.Controls
{
	using Model;
	using UI;

	public class RepeaterView<T> : StackLayout
	{
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
			"ItemsSource", typeof(IReadOnlyList<T>),
			typeof(RepeaterView<T>),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnItemsSourceChanged,
			defaultValue: null);

		public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
			"ItemTemplate", typeof(DataTemplate),
			typeof(RepeaterView<T>),
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: OnItemTemplateChanged,
			defaultValue: default(DataTemplate));

		public RepeaterView()
		{
			this.Spacing = 0;
		}

		public delegate void ItemAddedEventHandler(object sender, RepeaterViewItemAddedEventArgs args);

		public event ItemAddedEventHandler ItemCreated;

		public IReadOnlyList<T> ItemsSource
		{
			get { return (IReadOnlyList<T>)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}

		protected virtual void NotifyItemAdded(View view, object model)
		{
			ItemCreated?.Invoke(this, new RepeaterViewItemAddedEventArgs(view, model));
		}

		private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var control = bindable as RepeaterView<T>;

			if (control.ItemTemplate != null)
			{
				control.InitializeItems();
			}
		}

		private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var control = bindable as RepeaterView<T>;

			if (control.ItemsSource != null)
			{
				control.InitializeItems();
			}
		}

		private void InitializeItems()
		{
			Children.Clear();

			foreach (var item in ItemsSource)
			{
				Children.Add(CreateView(item));
			}

			if (ItemsSource is INotifyCollectionChanged ncc)
			{
				ncc.CollectionChanged += OnCollectionChanged;
			}

			UpdateChildrenLayout();
			InvalidateLayout();
		}

		private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null)
			{
				Children.RemoveAt(e.OldStartingIndex);
				UpdateChildrenLayout();
				InvalidateLayout();
			}

			if (e.NewItems != null)
			{
				foreach (T item in e.NewItems)
				{
					var view = CreateView(item);

					Children.Insert(ItemsSource.IndexOf(item), view);
					NotifyItemAdded(view, item);
				}

				UpdateChildrenLayout();
				InvalidateLayout();
			}
		}

		private View CreateView(T item)
		{
			var cell = ItemTemplate.CreateContent();

			var view = ToView(cell);

			view.BindingContext = item;

			return view;
		}

		private View ToView(object cell)
		{
			switch (cell)
			{
				case ViewCell viewCell:
					return viewCell.View;

				case View view:
					return view;

				default:
					throw new ArgumentException();
			}
		}
	}

	public class RepeaterViewItemAddedEventArgs : EventArgs
	{
		public RepeaterViewItemAddedEventArgs(View view, object model)
		{
			View = view;
			Model = model;
		}

		public View View { get; set; }
		public object Model { get; set; }
	}

	public class AccountEntryList : RepeaterView<AccountEntryVM>
	{ }

	public class AccountChangeList : RepeaterView<TokenStore.Change>
	{ }
}