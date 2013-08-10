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
            Log.Info("Activate called on {0}-{1}", GetType().FullName, DisplayName);
            Log.Info("Active value - {0}", _isActive);
            if (_isActive) return;

            _isActive = true;
            Log.Info("Active value - {0}", _isActive);

            Log.Info("Calling OnInitialise");
            if (!_onInitialiseHasBeenCalled)
            {
                OnInitialise();
                _onInitialiseHasBeenCalled = true;
            }
        }

        void IViewModel.DeActivate()
        {
            _isActive = false;
        }

        protected virtual void OnInitialise()
        { }
    }
}