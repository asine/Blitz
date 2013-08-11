using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blitz.Client.Common.ReportViewer
{
    public interface IReportViewerService<TRequest, TResponse>
    {
        TRequest CreateRequest();

        Task<TResponse> GetHistory(TRequest request);

        Task<List<ReportViewerItemViewModel>> GenerateItemViewModels(TResponse response);

        void OnActivate();

        void OnDeActivate();

        void CleanUp();
    }
}