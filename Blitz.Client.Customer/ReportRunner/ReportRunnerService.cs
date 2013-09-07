using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.DynamicReportData;
using Blitz.Client.Common.ExportToExcel;
using Blitz.Client.Common.ReportRunner;
using Blitz.Client.Core.Agatha;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.TPL;
using Blitz.Client.Customer.ReportLayout;
using Blitz.Client.Customer.ReportParameters;
using Blitz.Common.Core;
using Blitz.Common.Customer;

namespace Blitz.Client.Customer.ReportRunner
{
    public interface IReportRunnerService : IReportRunnerService<ReportParameterViewModel, ReportRunnerRequest, ReportRunnerResponse>
    {
        Task ShowLayoutAsync();
    }

    public class ReportRunnerService : ReportRunnerService<ReportParameterViewModel, ReportRunnerRequest, ReportRunnerResponse>, IReportRunnerService
    {
        private readonly Func<DynamicReportDataViewModel> _simpleReportDataViewModelFactory;
        private readonly IRequestTask _requestTask;
        private readonly IViewService _viewService;
        private readonly Func<ReportLayoutViewModel> _reportLayoutViewModelFactory;
        private readonly IBasicExportToExcel _exportToExcel;

        public ReportRunnerService(Func<DynamicReportDataViewModel> simpleReportDataViewModelFactory,
            IRequestTask requestTask, IViewService viewService, ILog log,
            Func<ReportLayoutViewModel> reportLayoutViewModelFactory, IBasicExportToExcel exportToExcel)
            : base(log)
        {
            _simpleReportDataViewModelFactory = simpleReportDataViewModelFactory;
            _requestTask = requestTask;
            _viewService = viewService;
            _reportLayoutViewModelFactory = reportLayoutViewModelFactory;
            _exportToExcel = exportToExcel;
        }

        public override Task ConfigureParameterViewModelAsync(ReportParameterViewModel viewModel)
        {
            return _requestTask.Get<InitialiseParametersRequest, InitialiseParametersResponse>(new InitialiseParametersRequest())
                .SelectMany(x =>
                {
                    var availableDates = x.AvailableDates.OrderByDescending(d => d);
                    foreach (var availableDate in availableDates)
                    {
                        viewModel.Dates.Add(availableDate);
                    }

                    viewModel.SelectedDate = availableDates.First();
                });
        }

        public override ReportRunnerRequest CreateRequest(ReportParameterViewModel reportParameterViewModel)
        {
            return new ReportRunnerRequest { ReportDate = reportParameterViewModel.SelectedDate };
        }

        public override Task<ReportRunnerResponse> GenerateAsync(ReportRunnerRequest request)
        {
            return _requestTask.Get<ReportRunnerRequest, ReportRunnerResponse>(request);
        }

        public override Task<List<IViewModel>> GenerateDataViewModelsAsync(ReportRunnerResponse response)
        {
            return Task.Factory.StartNew(() => new List<IViewModel>(response.Results
                .Select((x, i) =>
                {
                    var dataViewModel = _simpleReportDataViewModelFactory();
                    dataViewModel.DisplayName = "ReportData " + i;

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