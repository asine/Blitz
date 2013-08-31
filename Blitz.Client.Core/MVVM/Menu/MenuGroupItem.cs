using Microsoft.Practices.Prism.ViewModel;

namespace Blitz.Client.Core.MVVM.Menu
{
    public class MenuGroupItem : NotificationObject, IMenuItem
    {
        public string DisplayName { get; set; }

        #region IsVisible

        private bool _isVisible;

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value.Equals(_isVisible)) return;
                _isVisible = value;
                RaisePropertyChanged(() => IsVisible);
            }
        }

        #endregion

        public string ImageName { get; set; }

        public BindableCollection<IMenuItem> Items { get; private set; }

        public MenuGroupItem(BindableCollectionFactory bindableCollectionFactory)
        {
            Items = bindableCollectionFactory.Get<IMenuItem>();

            IsVisible = true;
        }
    }
}