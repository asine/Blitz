using System;
using System.Threading.Tasks;

namespace Blitz.Client.Core.MVVM
{
    public interface IDispatcherService
    {
        void ExecuteSyncOnUI(Action action);

        Task ExecuteAsyncOnUI(Action action);
    }
}