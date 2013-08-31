using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.ReportData.Simple;
using Blitz.Client.Common.ReportViewer;
using Blitz.Client.Common.ReportViewer.History;
using Blitz.Client.Core.Agatha;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Common.Core;
using Blitz.Common.Customer;

namespace Blitz.Client.Customer.Reportviewer
{
    public class ReportViewerService : ReportViewerService<GetHistoryListRequest, GetHistoryListResponse, GetHistoryReportsRequest, GetHistoryReportsResponse>
    {
        private readonly IRequestTask _requestTask;
        private readonly Func<SimpleReportDataViewModel> _simpleReportDataViewModelFactory;

        public ReportViewerService(IToolBarService toolBarService, ILog log, IRequestTask requestTask, 
            Func<SimpleReportDataViewModel> simpleReportDataViewModelFactory) 
            : base(toolBarService, log)
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
                    dataViewModel.DisplayName = "ReportData " + i;

                    for (var index = 0; index < 100; index++)
                    {
                        var item = new ReportDto { Id = index };
                        dataViewModel.Items.Add(item);
                    }

                    return dataViewModel;
                })
                .ToList()));
        }
    }
}