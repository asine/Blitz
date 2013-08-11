using Blitz.Common.Core;

using Microsoft.Practices.Prism.ViewModel;

namespace Blitz.Client.Core.MVVM
{
    public abstract class ViewModelBase : NotificationObject, IViewModel
    {
        protected readonly ILog Log;

        #region DisplayName

        private string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (value == _displayName) return;
                _displayName = value;
                RaisePropertyChanged(() => DisplayName);
            }
        }

        #endregion

        protected ViewModelBase(ILog log)
        {
            Log = log;
        }
    }
}