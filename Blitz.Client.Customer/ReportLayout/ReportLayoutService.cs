﻿using System.Threading.Tasks;

using Blitz.Common.Customer;

using Naru.Agatha;

namespace Blitz.Client.Customer.ReportLayout
{
    public interface IReportLayoutService
    {
        Task<GetAttributesResponse> GetAttributesAsync();
    }

    public class ReportLayoutService : IReportLayoutService
    {
        private readonly IRequestTask _requestTask;

        public ReportLayoutService(IRequestTask requestTask)
        {
            _requestTask = requestTask;
        }

        public Task<GetAttributesResponse> GetAttributesAsync()
        {
            return _requestTask.Get(new GetAttributesRequest());
        }
    }
}