using OfficeOpenXml;

namespace Blitz.Client.Core.EPPlus
{
    public class ExcelWorkSheetWriter
    {
        public ExcelWorksheet Create(ExcelPackage excelPackage, ExcelWorkSheetModel excelWorkSheetModel)
        {
            return excelPackage.Workbook.Worksheets.Add(excelWorkSheetModel.Title);
        }
    }
}