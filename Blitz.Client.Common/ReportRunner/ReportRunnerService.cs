using System.Collections.Generic;
using System.Threading.Tasks;

using Naru.WPF.ViewModel;

namespace Blitz.Client.Common.ReportRunner
{
    public abstract class ReportRunnerService<TReportParameterViewModel, TRequest, TResponse>
        : Service, IReportRunnerService<TReportParameterViewModel, TRequest, TResponse>
    {
        public abstract Task ConfigureParameterViewModelAsync(TReportParameterViewModel viewModel);

        public abstract TRequest CreateRequest(TReportParameterViewModel reportParameterViewModel);

        public abstract Task<TResponse> GenerateAsync(TRequest request);

        public abstract Task<List<IViewModel>> GenerateDataViewModelsAsync(TResponse response);

        public abstract void ExportToExcel(TResponse response);
    }
}