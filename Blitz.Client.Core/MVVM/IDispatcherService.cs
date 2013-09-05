using System;
using System.Threading.Tasks;

using Blitz.Client.Core.TPL;

namespace Blitz.Client.Core.MVVM
{
    public interface IDispatcherService
    {
        void ExecuteSyncOnUI(Action action);

        Task<Unit> ExecuteAsyncOnUI(Action action);
    }
}