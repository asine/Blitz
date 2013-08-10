using Blitz.Common.Core;

using Microsoft.Practices.Prism.ViewModel;

namespace Blitz.Client.Core.MVVM
{
    public abstract class ViewModelBase : NotificationObject, IViewModel
    {
        protected readonly ILog Log;

        private bool _onInitialiseHasBeenCalled;
        private bool _isActive;

        public string DisplayName { get; set; }

        protected ViewModelBase(ILog log)
        {
            Log = log;
        }

        void IViewModel.Activate()
        {
            Log.Info("Activate called on {0} - {1}", GetType().FullName, DisplayName);
            Log.Info("Active value - {0}", _isActive);
            if (_isActive) return;

            _isActive = true;
            Log.Info("Active value - {0}", _isActive);

            if (_onInitialiseHasBeenCalled) return;

            Log.Info("Calling OnInitialise on {0} - {1}", GetType().FullName, DisplayName);
            OnInitialise();
            _onInitialiseHasBeenCalled = true;
        }

        void IViewModel.DeActivate()
        {
            _isActive = false;

            Log.Info("DeActivate called on {0} - {1}", GetType().FullName, DisplayName);
            Log.Info("DeActivate value - {0}", _isActive);
        }

        protected virtual void OnInitialise()
        { }
    }
}