using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Naru.Agatha;
using Naru.TPL;

using Blitz.Common.Trading.Security.Chart;

using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Trading.Security.Chart
{
    public interface IChartService : IService
    {
        Task<List<HistoricalDataDto>> GetDataAsync(string ticker, DateTime from, DateTime to);
    }

    public class ChartService : Service, IChartService
    {
        private readonly IRequestTask _requestTask;
        private readonly ISchedulerProvider _scheduler;

        public ChartService(IRequestTask requestTask, ISchedulerProvider scheduler)
        {
            _requestTask = requestTask;
            _scheduler = scheduler;
        }

        public Task<List<HistoricalDataDto>> GetDataAsync(string ticker, DateTime from, DateTime to)
        {
            var request = new GetHistoricDataRequest
            {
                Ticker = ticker,
                From = from,
                To = to
            };

            return _requestTask
                .Get(request)
                .Select(x => x.Results.ToList(), _scheduler.Task.TPL);
        }
    }
}