using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Client.Common.ReportData.Simple;
using Blitz.Client.Common.ReportParameter.Simple;
using Blitz.Client.Common.ReportRunner;
using Blitz.Client.Core.Agatha;
using Blitz.Client.Core.MVVM;
using Blitz.Common.Customer;

namespace Blitz.Client.Customer
{
    public class CustomerReportRunnerService : IReportRunnerService<SimpleReportParameterViewModel, ReportRunnerRequest, ReportRunnerResponse>
    {
        private readonly Func<SimpleReportDataViewModel> _simpleReportDataViewModelFactory;
        private readonly IRequestTask _requestTask;

        public CustomerReportRunnerService(Func<SimpleReportDataViewModel> simpleReportDataViewModelFactory,
            IRequestTask requestTask)
        {
            _simpleReportDataViewModelFactory = simpleReportDataViewModelFactory;
            _requestTask = requestTask;
        }

        public Task ConfigureParameterViewModel(SimpleReportParameterViewModel viewModel)
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

        public ReportRunnerRequest CreateRequest(SimpleReportParameterViewModel reportParameterViewModel)
        {
            return new ReportRunnerRequest {ReportDate = reportParameterViewModel.SelectedDate};
        }

        public Task<ReportRunnerResponse> Generate(ReportRunnerRequest request)
        {
            return _requestTask.GetUnstarted<ReportRunnerRequest, ReportRunnerResponse>(request);
        }

        public Task<List<IViewModel>> GenerateDataViewModels(ReportRunnerResponse response)
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
    }
}