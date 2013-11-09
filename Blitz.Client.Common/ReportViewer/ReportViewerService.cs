using System.Collections.Generic;
using System.Threading.Tasks;

using Blitz.Client.Common.ReportViewer.History;

using Common.Logging;

using Naru.WPF.ViewModel;

namespace Blitz.Client.Common.ReportViewer
{
    public abstract class ReportViewerService<THistoryRequest, THistoryResponse, TReportRequest, TReportResponse> 
        : Service, IReportViewerService<THistoryRequest, THistoryResponse, TReportRequest, TReportResponse>
    {
        protected ReportViewerService(ILog log)
            : base(log)
        { }

        public abstract THistoryRequest CreateHistoryRequest();

        public abstract Task<THistoryResponse> GetHistoryAsync(THistoryRequest request);

        public abstract Task<List<HistoryItemViewModel>> GenerateHistoryItemViewModelsAsync(THistoryResponse response);

        public abstract TReportRequest CreateReportRequest(long id);

        public abstract Task<TReportResponse> GenerateReportAsync(TReportRequest request);

        public abstract Task<List<IViewModel>> GenerateReportViewModelsAsync(TReportResponse response);
    }
}