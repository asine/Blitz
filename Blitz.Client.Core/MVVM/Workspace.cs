using System;
using System.Threading.Tasks;

using Blitz.Common.Core;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

namespace Blitz.Client.Core.MVVM
{
    public abstract class Workspace : ViewModel, ISupportClosing, ISupportActivationState
    {
        protected readonly IDispatcherService DispatcherService;
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

        public DelegateCommand ClosingCommand { get; private set; }

        protected Workspace(ILog log, IDispatcherService dispatcherService) 
            : base(log)
        {
            DispatcherService = dispatcherService;
            Disposables = new CompositeDisposable();

            ClosingCommand = new DelegateCommand(Close);
        }

        #region SupportClosing

        public void Close()
        {
            Log.Info("Closing ViewModel {0} - {1}", GetType().FullName, DisplayName);

            Closing();

            Disposables.Dispose();

            CleanUp();

            Closed.SafeInvoke(this);
        }

        public event EventHandler Closed;

        protected virtual void Closing()
        { }

        protected virtual void CleanUp()
        {
        }

        #endregion

        #region SupportAsyncOperations

        protected void Busy(string message)
        {
            IsBusy = true;
            BusyMessage = message;
        }

        protected void Idle()
        {
            IsBusy = false;
            BusyMessage = string.Empty;
        }

        protected Task<Unit> BusyAsync(string message)
        {
            return DispatcherService.ExecuteAsyncOnUI(() => Busy(message));
        }

        protected Task<Unit> IdleAsync()
        {
            return DispatcherService.ExecuteAsyncOnUI(() => Idle());
        }

        #endregion

        #region SupportActivationState

        private bool _onInitialiseHasBeenCalled;

        public bool IsActive { get; private set; }

        void ISupportActivationState.Activate()
        {
            Log.Info("Activate called on {0} - {1}", GetType().FullName, DisplayName);
            Log.Info("Active value - {0}", IsActive);
            if (IsActive) return;

            IsActive = true;
            Log.Info("Active value - {0}", IsActive);

            ActivationStateChanged.SafeInvoke(this, new DataEventArgs<bool>(IsActive));

            OnActivate();

            if (_onInitialiseHasBeenCalled) return;

            Log.Info("Calling OnInitialise on {0} - {1}", GetType().FullName, DisplayName);
            OnInitialise();
            _onInitialiseHasBeenCalled = true;
        }

        void ISupportActivationState.DeActivate()
        {
            IsActive = false;

            Log.Info("DeActivate called on {0} - {1}", GetType().FullName, DisplayName);
            Log.Info("DeActivate value - {0}", IsActive);

            ActivationStateChanged.SafeInvoke(this, new DataEventArgs<bool>(IsActive));

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