using System.Collections.Generic;

namespace Blitz.Client.Common.ExportToExcel
{
    public interface IExportToExcel<T>
    {
        void ExportToExcel(IEnumerable<IEnumerable<T>> reports);
    }
}