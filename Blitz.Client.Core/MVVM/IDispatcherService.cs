using System;
using System.Threading.Tasks;

namespace Blitz.Client.Core.MVVM
{
    public interface IDispatcherService
    {
        Task<Unit> ExecuteSyncOnUI(Action action);

        Task<Unit> ExecuteAsyncOnUI(Action action);
    }
}