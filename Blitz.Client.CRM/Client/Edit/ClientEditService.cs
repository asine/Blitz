using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Common.Logging;

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
        private readonly ISchedulerProvider _scheduler;

        public ClientEditService(ILog log, ISchedulerProvider scheduler)
            : base(log)
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