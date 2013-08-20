using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Blitz.Client.Core.MVVM
{
    public class DispatcherService : IDispatcherService
    {
        // Taken from Caliburn.Micro

        private readonly Dispatcher _dispatcher;

        public DispatcherService(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void ExecuteSyncOnUI(Action action)
        {
            if (_dispatcher == null || _dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Exception exception = null;

                Action method = () =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                };

                _dispatcher.Invoke(method);

                if (exception != null)
                {
                    throw new TargetInvocationException("An error occurred while dispatching a call to the UI Thread", exception);
                }
            }
        }

        public Task<Unit> ExecuteAsyncOnUI(Action action)
        {
            var taskSource = new TaskCompletionSource<Unit>();

            Action method = () =>
            {
                try
                {
                    action();
                    taskSource.SetResult(Unit.Default);
                }
                catch (Exception ex)
                {
                    taskSource.SetException(ex);
                }
            };

            _dispatcher.BeginInvoke(method);

            return taskSource.Task;
        }
    }
}