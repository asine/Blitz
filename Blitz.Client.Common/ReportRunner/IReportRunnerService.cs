using System.Collections.Generic;
using System.Threading.Tasks;

using Blitz.Client.Core.MVVM;

namespace Blitz.Client.Common.ReportRunner
{
    public interface IReportRunnerService<TReportParameterViewModel, TRequest, TResponse>
    {
        Task ConfigureParameterViewModel(TReportParameterViewModel viewModel);

        TRequest CreateRequest(TReportParameterViewModel reportParameterViewModel);

        Task<TResponse> Generate(TRequest request);

        Task<List<IViewModel>> GenerateDataViewModels(TResponse response);
    }
}