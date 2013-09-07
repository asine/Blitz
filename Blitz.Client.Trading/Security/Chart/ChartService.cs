using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Core.Agatha;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.TPL;
using Blitz.Common.Core;
using Blitz.Common.Trading.Security.Chart;

namespace Blitz.Client.Trading.Security.Chart
{
    public interface IChartService : IService
    {
        Task<List<HistoricalDataDto>> GetData(string ticker, DateTime from, DateTime to);
    }

    public class ChartService : Service, IChartService
    {
        private readonly IRequestTask _requestTask;

        public ChartService(ILog log, IRequestTask requestTask)
            : base(log)
        {
            _requestTask = requestTask;
        }

        public Task<List<HistoricalDataDto>> GetData(string ticker, DateTime from, DateTime to)
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