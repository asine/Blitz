using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.DynamicReportData;
using Blitz.Client.Common.ExportToExcel;
using Blitz.Client.Common.ReportRunner;

using Common.Logging;

using Naru.Agatha;
using Naru.TPL;
using Naru.WPF.MVVM;

using Blitz.Client.Customer.ReportLayout;
using Blitz.Client.Customer.ReportParameters;
using Blitz.Common.Customer;

using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Customer.ReportRunner
{
    public interface IReportRunnerService : IReportRunnerService<ReportParameterViewModel, ReportRunnerRequest, ReportRunnerResponse>
    {
        Task ShowLayoutAsync();
    }

    public class ReportRunnerService : ReportRunnerService<ReportParameterViewModel, ReportRunnerRequest, ReportRunnerResponse>, IReportRunnerService
    {
        private readonly Func<DynamicReportDataViewModel> _dynamicReportDataViewModelFactory;
        private readonly IRequestTask _requestTask;
        private readonly IViewService _viewService;
        private readonly Func<ReportLayoutViewModel> _reportLayoutViewModelFactory;
        private readonly IBasicExportToExcel _exportToExcel;
        private readonly ISchedulerProvider _scheduler;

        public ReportRunnerService(Func<DynamicReportDataViewModel> dynamicReportDataViewModelFactory,
                                   IRequestTask requestTask, IViewService viewService, ILog log,
                                   Func<ReportLayoutViewModel> reportLayoutViewModelFactory,
                                   IBasicExportToExcel exportToExcel,
            ISchedulerProvider scheduler)
            : base(log)
        {
            _dynamicReportDataViewModelFactory = dynamicReportDataViewModelFactory;
            _requestTask = requestTask;
            _viewService = viewService;
            _reportLayoutViewModelFactory = reportLayoutViewModelFactory;
            _exportToExcel = exportToExcel;
            _scheduler = scheduler;
        }

        public override Task ConfigureParameterViewModelAsync(ReportParameterViewModel viewModel)
        {
            return CompletedTask.Default;
        }

        public override ReportRunnerRequest CreateRequest(ReportParameterViewModel reportParameterViewModel)
        {
            return new ReportRunnerRequest { ReportDate = reportParameterViewModel.Context.SelectedDate };
        }

        public override Task<ReportRunnerResponse> GenerateAsync(ReportRunnerRequest request)
        {
            return _requestTask.Get(request);
        }

        public override Task<List<IViewModel>> GenerateDataViewModelsAsync(ReportRunnerResponse response)
        {
            return Task.Factory.StartNew(() => new List<IViewModel>(response.Results
                .Select((x, i) =>
                {
                    var dataViewModel = _dynamicReportDataViewModelFactory();
                    dataViewModel.SetupHeader("ReportData " + i);

                    var items = Enumerable.Range(0, 100)
                        .Select(index => new ReportDto {Id = index, Name = "Name " + index});
                    dataViewModel.Initialise(items);

                    return dataViewModel;
                })
                .ToList()));
        }

        public override void ExportToExcel(ReportRunnerResponse response)
        {
            var sheets = new List<List<ReportDto>>();

            foreach (var reportDto in response.Results)
            {
                var results = new List<ReportDto>();

                for (var index = 0; index < 100; index++)
                {
                    var item = new ReportDto { Id = index };
                    results.Add(item);
                }

                sheets.Add(results);
            }

            _exportToExcel.ExportToExcel(sheets);
        }

        public Task ShowLayoutAsync()
        {
            return _viewService.ShowModalAsync(_reportLayoutViewModelFactory());
        }
    }
}