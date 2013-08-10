using Blitz.Common.Core;

namespace Blitz.Client.Core.MVVM
{
    public abstract class Workspace : ViewModelBase, ISupportClosing
    {
        protected readonly CompositeDisposable Disposables;

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
    }
}