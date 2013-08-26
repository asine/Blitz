﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.ExportToExcel;
using Blitz.Client.Common.ReportData.Simple;
using Blitz.Client.Common.ReportParameter.Simple;
using Blitz.Client.Common.ReportRunner;
using Blitz.Client.Core;
using Blitz.Client.Core.Agatha;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Client.Customer.ReportLayout;
using Blitz.Common.Core;
using Blitz.Common.Customer;

using Microsoft.Practices.Prism.Commands;

namespace Blitz.Client.Customer
{
    public class CustomerReportRunnerService : ReportRunnerService<SimpleReportParameterViewModel, ReportRunnerRequest, ReportRunnerResponse>
    {
        private readonly Func<SimpleReportDataViewModel> _simpleReportDataViewModelFactory;
        private readonly IRequestTask _requestTask;
        private readonly IToolBarService _toolBarService;
        private readonly Func<ReportLayoutViewModel> _reportLayoutViewModelFactory;
        private readonly IBasicExportToExcel _exportToExcel;

        private readonly List<IToolBarItem> _toolBarItems;

        public CustomerReportRunnerService(Func<SimpleReportDataViewModel> simpleReportDataViewModelFactory,
            IRequestTask requestTask, IToolBarService toolBarService, IViewService viewService, ILog log,
            Func<ReportLayoutViewModel> reportLayoutViewModelFactory, IBasicExportToExcel exportToExcel)
        {
            _simpleReportDataViewModelFactory = simpleReportDataViewModelFactory;
            _requestTask = requestTask;
            _toolBarService = toolBarService;
            _reportLayoutViewModelFactory = reportLayoutViewModelFactory;
            _exportToExcel = exportToExcel;

            _toolBarItems = new List<IToolBarItem> {CreateShowLayoutToolBarItem(viewService)};

            foreach (var toolBarItem in _toolBarItems)
            {
                _toolBarService.Items.Add(toolBarItem);
            }
        }

        public override Task ConfigureParameterViewModel(SimpleReportParameterViewModel viewModel)
        {
            return _requestTask.Get<InitialiseParametersRequest, InitialiseParametersResponse>(new InitialiseParametersRequest())
                .ThenDo(x =>
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

        public override Task<ReportRunnerResponse> Generate(ReportRunnerRequest request)
        {
            return _requestTask.Get<ReportRunnerRequest, ReportRunnerResponse>(request);
        }

        public override Task<List<IViewModel>> GenerateDataViewModels(ReportRunnerResponse response)
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

        private ToolBarButtonItem CreateShowLayoutToolBarItem(IViewService viewService)
        {
            return new ToolBarButtonItem
            {
                DisplayName = "Layout",
                Command = new DelegateCommand(() => ShowLayout(viewService)),
                IsVisible = false
            };
        }

        private void ShowLayout(IViewService viewService)
        {
            viewService.ShowModal(_reportLayoutViewModelFactory());
        }

        public override void OnActivate()
        {
            foreach (var toolBarItem in _toolBarItems)
            {
                toolBarItem.IsVisible = true;
            }
        }

        public override void OnDeActivate()
        {
            foreach (var toolBarItem in _toolBarItems)
            {
                toolBarItem.IsVisible = false;
            }
        }

        public override void CleanUp()
        {
            foreach (var toolBarItem in _toolBarItems)
            {
                _toolBarService.Items.Remove(toolBarItem);
            }
        }
    }
}