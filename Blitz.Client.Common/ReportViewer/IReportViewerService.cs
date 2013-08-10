using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blitz.Client.Common.ReportViewer
{
    public interface IReportViewerService<TRequest, TResponse>
    {
        TRequest CreateRequest();

        Task<TResponse> GetHistory(TRequest request);

        Task<List<ReportViewerItemViewModel>> GenerateDataViewModels(TResponse response);
    }
}