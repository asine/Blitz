using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Core.Agatha;

using Common.Logging;

using Naru.WPF.MVVM;

using Blitz.Common.Trading.Security.Chart;

using Naru.WPF.TPL;

namespace Blitz.Client.Trading.Security.Chart
{
    public interface IChartService : IService
    {
        Task<List<HistoricalDataDto>> GetDataAsync(string ticker, DateTime from, DateTime to);
    }

    public class ChartService : Service, IChartService
    {
        private readonly IRequestTask _requestTask;

        public ChartService(ILog log, IRequestTask requestTask)
            : base(log)
        {
            _requestTask = requestTask;
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
                .Get<GetHistoricDataRequest, GetHistoricDataResponse>(request)
                .Select(x => x.Results.ToList());
        }
    }
}