using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Naru.TPL;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Blitz.Client.CRM.Client.Edit
{
    public interface IClientEditService
    {
        Task<List<string>> GetGendersAsync();

        Task SaveAsync();
    }

    public class ClientEditService : Service, IClientEditService
    {
        private readonly IDispatcherSchedulerProvider _scheduler;

        public ClientEditService(IDispatcherSchedulerProvider scheduler)
        {
            _scheduler = scheduler;
        }

        public Task<List<string>> GetGendersAsync()
        {
            var genders = new List<string> {"Male", "Female", "Unknown"};
            return Task.FromResult(genders);
        }

        public Task SaveAsync()
        {
            return Task.Factory.StartNew(() => { Thread.Sleep(TimeSpan.FromSeconds(10)); }, _scheduler.Task.TPL);
        }
    }
}