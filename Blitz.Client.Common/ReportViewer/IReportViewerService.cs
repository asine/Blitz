using System.Collections.Generic;
using System.Threading.Tasks;

using Blitz.Client.Common.ReportViewer.History;

using Naru.WPF.ViewModel;

namespace Blitz.Client.Common.ReportViewer
{
    public interface IReportViewerService<THistoryRequest, THistoryResponse, TReportRequest, TReportResponse> : IService
    {
        THistoryRequest CreateHistoryRequest();

        Task<THistoryResponse> GetHistoryAsync(THistoryRequest request);

        Task<List<HistoryItemViewModel>> GenerateHistoryItemViewModelsAsync(THistoryResponse response);

        TReportRequest CreateReportRequest(long id);

        Task<TReportResponse> GenerateReportAsync(TReportRequest request);

        Task<List<IViewModel>> GenerateReportViewModelsAsync(TReportResponse response);
    }
}