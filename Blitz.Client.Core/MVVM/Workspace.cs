using Blitz.Common.Core;

namespace Blitz.Client.Core.MVVM
{
    public abstract class Workspace : ViewModelBase, ISupportClosing
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

        public void Close()
        {
            Log.Info("Closing ViewModel {0} - {1}", GetType().FullName, DisplayName);

            Disposables.Dispose();

            CleanUp();
        }

        protected virtual void CleanUp()
        { }

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
    }
}