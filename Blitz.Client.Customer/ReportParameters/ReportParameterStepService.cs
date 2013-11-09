using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Common.Customer;

using Common.Logging;

using Naru.Agatha;
using Naru.TPL;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Customer.ReportParameters
{
    public interface IReportParameterStepService
    {
        Task<List<DateTime>> GetAvailableDatesAsync();
    }

    public class ReportParameterStepService : Service, IReportParameterStepService
    {
        private readonly IRequestTask _requestTask;

        public ReportParameterStepService(ILog log, IRequestTask requestTask) 
            : base(log)
        {
            _requestTask = requestTask;
        }

        public Task<List<DateTime>> GetAvailableDatesAsync()
        {
            return _requestTask.Get(new InitialiseParametersRequest())
                .Select(x => x.AvailableDates.ToList());
        }
    }
}