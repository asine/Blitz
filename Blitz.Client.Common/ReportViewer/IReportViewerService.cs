using System.Collections.Generic;
using System.Threading.Tasks;

using Blitz.Client.Common.ReportViewer.History;
using Blitz.Client.Core.MVVM;

namespace Blitz.Client.Common.ReportViewer
{
    public interface IReportViewerService<THistoryRequest, THistoryResponse, TReportRequest, TReportResponse>
    {
        THistoryRequest CreateHistoryRequest();

        Task<THistoryResponse> GetHistory(THistoryRequest request);

        Task<List<HistoryItemViewModel>> GenerateHistoryItemViewModels(THistoryResponse response);

        TReportRequest CreateReportRequest(long id);

        Task<TReportResponse> GenerateReport(TReportRequest request);

        Task<List<IViewModel>> GenerateReportViewModels(TReportResponse response);

        void OnActivate();

        void OnDeActivate();

        void CleanUp();
    }
}