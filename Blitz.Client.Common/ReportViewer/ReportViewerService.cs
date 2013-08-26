using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blitz.Client.Common.ReportViewer
{
    public abstract class ReportViewerServiceBase<TRequest, TResponse> : IReportViewerService<TRequest, TResponse>
    {
        public abstract TRequest CreateRequest();

        public abstract Task<TResponse> GetHistory(TRequest request);

        public abstract Task<List<ReportViewerItemViewModel>> GenerateItemViewModels(TResponse response);

        public virtual void OnActivate()
        { }

        public virtual void OnDeActivate()
        { }

        public virtual void CleanUp()
        { }
    }
}