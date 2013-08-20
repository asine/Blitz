using System.Collections.Generic;
using System.Threading.Tasks;

using Blitz.Client.Core;
using Blitz.Client.Core.MVVM;

namespace Blitz.Client.Common.ReportRunner
{
    public abstract class ReportRunnerServiceBase<TReportParameterViewModel, TRequest, TResponse> :
        IReportRunnerService<TReportParameterViewModel, TRequest, TResponse>
    {
        public abstract Task<Unit> ConfigureParameterViewModel(TReportParameterViewModel viewModel);

        public abstract TRequest CreateRequest(TReportParameterViewModel reportParameterViewModel);

        public abstract Task<TResponse> Generate(TRequest request);

        public abstract Task<List<IViewModel>> GenerateDataViewModels(TResponse response);

        public virtual void OnActivate()
        { }

        public virtual void OnDeActivate()
        { }

        public virtual void CleanUp()
        { }
    }
}