using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace Blitz.Client.Core.MVVM.ToolBar
{
    public class ToolBarButtonItem : NotificationObject, IToolBarItem
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

        public DelegateCommand Command { get; set; }
        
        public string ImageName { get; set; }

        public ToolBarButtonItem()
        {
            IsVisible = true;
        }
    }
}