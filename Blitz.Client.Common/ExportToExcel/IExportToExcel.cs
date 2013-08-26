using System.Collections.Generic;

namespace Blitz.Client.Common.ReportRunner
{
    public interface IExportToExcel<T>
    {
        void ExportToExcel(IEnumerable<IEnumerable<T>> reports);
    }
}