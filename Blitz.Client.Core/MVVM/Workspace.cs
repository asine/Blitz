using System;

using Blitz.Common.Core;

using Microsoft.Practices.Prism.Events;

namespace Blitz.Client.Core.MVVM
{
    public abstract class Workspace : ViewModelBase, ISupportClosing, ISupportActivationState
    {
        protected readonly CompositeDisposable Disposables;

        #region IsBusy

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                if (value.Equals(_isBusy)) return;
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        #endregion

        #region BusyMessage

        private string _busyMessage;

        public string BusyMessage
        {
            get { return _busyMessage; }
            private set
            {
                if (value == _busyMessage) return;
                _busyMessage = value;
                RaisePropertyChanged(() => BusyMessage);
            }
        }

        #endregion

        protected Workspace(ILog log) 
            : base(log)
        {
            Disposables = new CompositeDisposable();
        }

        #region ISupportClosing

        public void Close()
        {
            Log.Info("Closing ViewModel {0} - {1}", GetType().FullName, DisplayName);

            Disposables.Dispose();

            CleanUp();
        }

        protected virtual void CleanUp()
        {
        }

        #endregion

        protected void BusyIndicatorSet(string message = "")
        {
            IsBusy = true;
            BusyMessage = message;
        }

        protected void BusyIndicatorClear()
        {
            IsBusy = false;
            BusyMessage = string.Empty;
        }

        #region SupportActivationState

        private bool _onInitialiseHasBeenCalled;

        private bool _isActive;

        void ISupportActivationState.Activate()
        {
            Log.Info("Activate called on {0} - {1}", GetType().FullName, DisplayName);
            Log.Info("Active value - {0}", _isActive);
            if (_isActive) return;

            _isActive = true;
            Log.Info("Active value - {0}", _isActive);

            ActivationStateChanged.SafeInvoke(this, new DataEventArgs<bool>(_isActive));

            OnActivate();

            if (_onInitialiseHasBeenCalled) return;

            Log.Info("Calling OnInitialise on {0} - {1}", GetType().FullName, DisplayName);
            OnInitialise();
            _onInitialiseHasBeenCalled = true;
        }

        void ISupportActivationState.DeActivate()
        {
            _isActive = false;

            Log.Info("DeActivate called on {0} - {1}", GetType().FullName, DisplayName);
            Log.Info("DeActivate value - {0}", _isActive);

            ActivationStateChanged.SafeInvoke(this, new DataEventArgs<bool>(_isActive));

            OnDeActivate();
        }

        public event EventHandler<DataEventArgs<bool>>  ActivationStateChanged;

        protected virtual void OnInitialise()
        { }

        protected virtual void OnActivate()
        { }

        protected virtual void OnDeActivate()
        { }

        #endregion
    }
}