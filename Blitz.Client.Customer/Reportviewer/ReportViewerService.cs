using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.DynamicReportData;
using Blitz.Client.Common.ReportViewer;
using Blitz.Client.Common.ReportViewer.History;

using Common.Logging;

using Naru.Agatha;

using Blitz.Common.Customer;

using Naru.TPL;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Customer.Reportviewer
{
    public interface IReportViewerService : IReportViewerService<GetHistoryListRequest, GetHistoryListResponse, GetHistoryReportsRequest, GetHistoryReportsResponse>
    {
    }

    public class ReportViewerService : ReportViewerService<GetHistoryListRequest, GetHistoryListResponse, GetHistoryReportsRequest, GetHistoryReportsResponse>, IReportViewerService
    {
        private readonly IRequestTask _requestTask;
        private readonly ISchedulerProvider _scheduler;
        private readonly Func<DynamicReportDataViewModel> _simpleReportDataViewModelFactory;

        public ReportViewerService(ILog log, IRequestTask requestTask, ISchedulerProvider scheduler, Func<DynamicReportDataViewModel> simpleReportDataViewModelFactory) 
            : base(log)
        {
            _requestTask = requestTask;
            _scheduler = scheduler;
            _simpleReportDataViewModelFactory = simpleReportDataViewModelFactory;
        }

        public override GetHistoryListRequest CreateHistoryRequest()
        {
            return new GetHistoryListRequest();
        }

        public override Task<GetHistoryListResponse> GetHistoryAsync(GetHistoryListRequest request)
        {
            return _requestTask.Get(request);
        }

        public override Task<List<HistoryItemViewModel>> GenerateHistoryItemViewModelsAsync(GetHistoryListResponse response)
        {
            return Task.Factory.StartNew(() =>
                                         {
                                             var historyItemViewModels = response.Results
                                                                                 .Select((x, i) =>
                                                                                         {
                                                                                             var item =  new HistoryItemViewModel();
                                                                                             item.Name = "Instance " + item.Name;

                                                                                             return item;
                                                                                         })
                                                                                 .ToList();
                                             return new List<HistoryItemViewModel>(historyItemViewModels);
                                         }, _scheduler.Task.TPL);
        }

        public override GetHistoryReportsRequest CreateReportRequest(long id)
        {
            return new GetHistoryReportsRequest {Id = id};
        }

        public override Task<GetHistoryReportsResponse> GenerateReportAsync(GetHistoryReportsRequest request)
        {
            return _requestTask.Get(request);
        }

        public override Task<List<IViewModel>> GenerateReportViewModelsAsync(GetHistoryReportsResponse response)
        {
            return Task.Factory.StartNew(() => new List<IViewModel>(response.Results
                .Select((x, i) =>
                {
                    var dataViewModel = _simpleReportDataViewModelFactory();
                    dataViewModel.SetupHeader(_scheduler, "ReportData " + i);

                    var items = Enumerable.Range(0, 100)
                        .Select(index => new ReportDto { Id = index });
                    dataViewModel.Initialise(items);

                    return dataViewModel;
                })
                .ToList()), _scheduler.Task.TPL);
        }
    }
}