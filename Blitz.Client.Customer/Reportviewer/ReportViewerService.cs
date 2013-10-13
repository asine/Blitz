using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.DynamicReportData;
using Blitz.Client.Common.ReportViewer;
using Blitz.Client.Common.ReportViewer.History;

using Common.Logging;

using Naru.Agatha;
using Naru.WPF.MVVM;

using Blitz.Common.Customer;

namespace Blitz.Client.Customer.Reportviewer
{
    public interface IReportViewerService : IReportViewerService<GetHistoryListRequest, GetHistoryListResponse, GetHistoryReportsRequest, GetHistoryReportsResponse>
    {
    }

    public class ReportViewerService : ReportViewerService<GetHistoryListRequest, GetHistoryListResponse, GetHistoryReportsRequest, GetHistoryReportsResponse>, IReportViewerService
    {
        private readonly IRequestTask _requestTask;
        private readonly Func<DynamicReportDataViewModel> _simpleReportDataViewModelFactory;

        public ReportViewerService(ILog log, IRequestTask requestTask, Func<DynamicReportDataViewModel> simpleReportDataViewModelFactory) 
            : base(log)
        {
            _requestTask = requestTask;
            _simpleReportDataViewModelFactory = simpleReportDataViewModelFactory;
        }

        public override GetHistoryListRequest CreateHistoryRequest()
        {
            return new GetHistoryListRequest();
        }

        public override Task<GetHistoryListResponse> GetHistoryAsync(GetHistoryListRequest request)
        {
            return _requestTask.Get<GetHistoryListRequest, GetHistoryListResponse>(request);
        }

        public override Task<List<HistoryItemViewModel>> GenerateHistoryItemViewModelsAsync(GetHistoryListResponse response)
        {
            return Task.Factory.StartNew(
                () => new List<HistoryItemViewModel>(response.Results
                    .Select((x, i) =>
                    {
                        var item = new HistoryItemViewModel();
                        item.Name = "Instance " + item.Name;

                        return item;
                    })
                    .ToList()));
        }

        public override GetHistoryReportsRequest CreateReportRequest(long id)
        {
            return new GetHistoryReportsRequest {Id = id};
        }

        public override Task<GetHistoryReportsResponse> GenerateReportAsync(GetHistoryReportsRequest request)
        {
            return _requestTask.Get<GetHistoryReportsRequest, GetHistoryReportsResponse>(request);
        }

        public override Task<List<IViewModel>> GenerateReportViewModelsAsync(GetHistoryReportsResponse response)
        {
            return Task.Factory.StartNew(() => new List<IViewModel>(response.Results
                .Select((x, i) =>
                {
                    var dataViewModel = _simpleReportDataViewModelFactory();
                    dataViewModel.SetupHeader("ReportData " + i);

                    var items = Enumerable.Range(0, 100)
                        .Select(index => new ReportDto { Id = index });
                    dataViewModel.Initialise(items);

                    return dataViewModel;
                })
                .ToList()));
        }
    }
}