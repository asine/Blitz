using System.Collections.Generic;
using System.Threading.Tasks;

using Blitz.Client.Common.ReportViewer.History;
using Blitz.Client.Core.MVVM;

namespace Blitz.Client.Common.ReportViewer
{
    public interface IReportViewerService<THistoryRequest, THistoryResponse, TReportRequest, TReportResponse>
    {
        THistoryRequest CreateHistoryRequest();

        Task<THistoryResponse> GetHistoryAsync(THistoryRequest request);

        Task<List<HistoryItemViewModel>> GenerateHistoryItemViewModelsAsync(THistoryResponse response);

        TReportRequest CreateReportRequest(long id);

        Task<TReportResponse> GenerateReportAsync(TReportRequest request);

        Task<List<IViewModel>> GenerateReportViewModelsAsync(TReportResponse response);

        void OnActivate();

        void OnDeActivate();

        void CleanUp();
    }
}