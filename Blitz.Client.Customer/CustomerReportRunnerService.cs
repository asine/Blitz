using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.ReportData.Simple;
using Blitz.Client.Common.ReportParameter.Simple;
using Blitz.Client.Common.ReportRunner;
using Blitz.Client.Core.Agatha;
using Blitz.Client.Core.MVVM;
using Blitz.Client.Core.MVVM.ToolBar;
using Blitz.Common.Customer;

namespace Blitz.Client.Customer
{
    public class CustomerReportRunnerService : ReportRunnerServiceBase<SimpleReportParameterViewModel, ReportRunnerRequest, ReportRunnerResponse>
    {
        private readonly Func<SimpleReportDataViewModel> _simpleReportDataViewModelFactory;
        private readonly IRequestTask _requestTask;
        private readonly IToolBarService _toolBarService;

        private readonly List<IToolBarItem> _toolBarItems;

        public CustomerReportRunnerService(Func<SimpleReportDataViewModel> simpleReportDataViewModelFactory,
            IRequestTask requestTask, IToolBarService toolBarService)
        {
            _simpleReportDataViewModelFactory = simpleReportDataViewModelFactory;
            _requestTask = requestTask;
            _toolBarService = toolBarService;

            _toolBarItems = new List<IToolBarItem>();
            _toolBarItems.Add(new ToolBarButtonItem {DisplayName = "Runner Test 1", IsVisible = false});

            foreach (var toolBarItem in _toolBarItems)
            {
                _toolBarService.Items.Add(toolBarItem);
            }
        }

        public override Task ConfigureParameterViewModel(SimpleReportParameterViewModel viewModel)
        {
            return _requestTask.Get<InitialiseParametersRequest, InitialiseParametersResponse>(new InitialiseParametersRequest())
                .ContinueWith(x =>
                {
                    var availableDates = x.Result.AvailableDates.OrderByDescending(d => d);
                    foreach (var availableDate in availableDates)
                    {
                        viewModel.Dates.Add(availableDate);
                    }

                    viewModel.SelectedDate = availableDates.First();
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public override ReportRunnerRequest CreateRequest(SimpleReportParameterViewModel reportParameterViewModel)
        {
            return new ReportRunnerRequest {ReportDate = reportParameterViewModel.SelectedDate};
        }

        public override Task<ReportRunnerResponse> Generate(ReportRunnerRequest request)
        {
            return _requestTask.GetUnstarted<ReportRunnerRequest, ReportRunnerResponse>(request);
        }

        public override Task<List<IViewModel>> GenerateDataViewModels(ReportRunnerResponse response)
        {
            return new Task<List<IViewModel>>(
                () => new List<IViewModel>(response.Results
                    .Select((x, i) =>
                    {
                        var dataViewModel = _simpleReportDataViewModelFactory();
                        dataViewModel.DisplayName = "ReportData " + i;

                        for (var index = 0; index < 100; index++)
                        {
                            var item = new ReportDto {Id = index};
                            dataViewModel.Items.Add(item);
                        }

                        return dataViewModel;
                    })
                    .ToList()));
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