using System.Collections.Generic;
using System.Threading.Tasks;

using Blitz.Client.Core.MVVM;

namespace Blitz.Client.Common.ReportRunner
{
    public interface IReportRunnerService<TReportParameterViewModel, TRequest, TResponse>
    {
        Task ConfigureParameterViewModelAsync(TReportParameterViewModel viewModel);

        TRequest CreateRequest(TReportParameterViewModel reportParameterViewModel);

        Task<TResponse> GenerateAsync(TRequest request);

        Task<List<IViewModel>> GenerateDataViewModelsAsync(TResponse response);

        void ExportToExcel(TResponse response); 

        void OnActivate();

        void OnDeActivate();

        void CleanUp();
    }
}