using System;
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

        public Task<Unit> ExecuteSyncOnUI(Action action)
        {
            var taskSource = new TaskCompletionSource<Unit>();

            if (_dispatcher == null || _dispatcher.CheckAccess())
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
            }
            else
            {
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

                _dispatcher.Invoke(method);
            }

            return taskSource.Task;
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