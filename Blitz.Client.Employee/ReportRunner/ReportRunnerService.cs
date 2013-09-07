﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.DynamicReportData;
using Blitz.Client.Common.ExportToExcel;
using Blitz.Client.Common.ReportParameter.Simple;
using Blitz.Client.Common.ReportRunner;
using Blitz.Client.Core;
using Blitz.Client.Core.Agatha;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Client.Core.TPL;
using Blitz.Common.Core;
using Blitz.Common.Customer;

namespace Blitz.Client.Employee.ReportRunner
{
    public class ReportRunnerService : ReportRunnerService<SimpleReportParameterViewModel, ReportRunnerRequest, ReportRunnerResponse>
    {
        private readonly Func<DynamicReportDataViewModel> _simpleReportDataViewModelFactory;
        private readonly IRequestTask _requestTask;
        private readonly IBasicExportToExcel _exportToExcel;

        public ReportRunnerService(Func<DynamicReportDataViewModel> simpleReportDataViewModelFactory,
            IRequestTask requestTask, IToolBarService toolBarService, ILog log, IBasicExportToExcel exportToExcel)
            : base(toolBarService, log)
        {
            _simpleReportDataViewModelFactory = simpleReportDataViewModelFactory;
            _requestTask = requestTask;
            _exportToExcel = exportToExcel;
        }

        public override Task ConfigureParameterViewModelAsync(SimpleReportParameterViewModel viewModel)
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

        public override ReportRunnerRequest CreateRequest(SimpleReportParameterViewModel reportParameterViewModel)
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
                        .Select(index => new ReportDto { Id = index });
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
    }
}