using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Blitz.Client.Core.MVVM
{
    public class BindableCollection<T> : ObservableCollection<T>
    {
        private readonly IDispatcherService _dispatcherService;

        public BindableCollection(IDispatcherService dispatcherService)
        {
            _dispatcherService = dispatcherService;
            IsNotifying = true;
        }

        public bool IsNotifying { get; set; }

        public virtual void NotifyOfPropertyChange(string propertyName)
        {
            if (IsNotifying)
                _dispatcherService.ExecuteSyncOnUI(() => OnPropertyChanged(new PropertyChangedEventArgs(propertyName)));
        }

        public void Refresh()
        {
            _dispatcherService.ExecuteSyncOnUI(() =>
            {
                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
        }

        protected override sealed void InsertItem(int index, T item)
        {
            _dispatcherService.ExecuteSyncOnUI(() => InsertItemBase(index, item));
        }

        protected virtual void InsertItemBase(int index, T item)
        {
            base.InsertItem(index, item);
        }

        protected override sealed void MoveItem(int oldIndex, int newIndex)
        {
            _dispatcherService.ExecuteSyncOnUI(() => MoveItemBase(oldIndex, newIndex));
        }

        protected virtual void MoveItemBase(int oldIndex, int newIndex)
        {
            base.MoveItem(oldIndex, newIndex);
        }

        protected override sealed void SetItem(int index, T item)
        {
            _dispatcherService.ExecuteSyncOnUI(() => SetItemBase(index, item));
        }

        protected virtual void SetItemBase(int index, T item)
        {
            base.SetItem(index, item);
        }

        protected override sealed void RemoveItem(int index)
        {
            _dispatcherService.ExecuteSyncOnUI(() => RemoveItemBase(index));
        }

        protected virtual void RemoveItemBase(int index)
        {
            base.RemoveItem(index);
        }

        protected override sealed void ClearItems()
        {
            _dispatcherService.ExecuteSyncOnUI(ClearItemsBase);
        }

        protected virtual void ClearItemsBase()
        {
            base.ClearItems();
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (IsNotifying)
            {
                base.OnCollectionChanged(e);
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (IsNotifying)
            {
                base.OnPropertyChanged(e);
            }
        }

        public virtual void AddRange(IEnumerable<T> items)
        {
            _dispatcherService.ExecuteSyncOnUI(() =>
            {
                var previousNotificationSetting = IsNotifying;
                IsNotifying = false;
                var index = Count;
                foreach (var item in items)
                {
                    InsertItemBase(index, item);
                    index++;
                }
                IsNotifying = previousNotificationSetting;

                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
        }

        public virtual void RemoveRange(IEnumerable<T> items)
        {
            _dispatcherService.ExecuteSyncOnUI(() =>
            {
                var previousNotificationSetting = IsNotifying;
                IsNotifying = false;
                foreach (var item in items)
                {
                    var index = IndexOf(item);
                    if (index >= 0)
                    {
                        RemoveItemBase(index);
                    }
                }
                IsNotifying = previousNotificationSetting;

                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
        }
    }
}