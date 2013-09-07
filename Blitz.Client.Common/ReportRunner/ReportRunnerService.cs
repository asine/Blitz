using System.Collections.Generic;
using System.Threading.Tasks;

using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Common.ReportRunner
{
    public abstract class ReportRunnerService<TReportParameterViewModel, TRequest, TResponse> : Service, IReportRunnerService<TReportParameterViewModel, TRequest, TResponse>
    {
        protected ReportRunnerService(ILog log)
            : base(log)
        { }

        public abstract Task ConfigureParameterViewModelAsync(TReportParameterViewModel viewModel);

        public abstract TRequest CreateRequest(TReportParameterViewModel reportParameterViewModel);

        public abstract Task<TResponse> GenerateAsync(TRequest request);

        public abstract Task<List<IViewModel>> GenerateDataViewModelsAsync(TResponse response);

        public abstract void ExportToExcel(TResponse response);

        public virtual void OnActivate()
        {
        }

        public virtual void OnDeActivate()
        {
        }

        public virtual void CleanUp()
        {
        }
    }
}