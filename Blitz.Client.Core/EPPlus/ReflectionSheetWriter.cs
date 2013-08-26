using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using OfficeOpenXml;

namespace Blitz.Client.Core.EPPlus
{
    public class ReflectionSheetWriter
    {
        public void Write(ExcelPackage excelPackage, SheetModel sheetModel, IEnumerable<object> records)
        {
            var sheet = CreateSheet(excelPackage, sheetModel);

            if (!records.Any()) return;

            var properties = records.First().GetType().GetProperties();

            var rowIndex = 1;

            WriteHeaders(sheet, properties, rowIndex);

            rowIndex++;

            WriteRecords(records, sheet, properties, rowIndex);
        }

        public void Write<T>(ExcelPackage excelPackage, SheetModel sheetModel, IEnumerable<T> records)
        {
            var sheet = CreateSheet(excelPackage, sheetModel);

            if (!records.Any()) return;

            var properties = typeof(T).GetProperties();

            var rowIndex = 1;

            WriteHeaders(sheet, properties, rowIndex);

            rowIndex++;

            WriteRecords(records, sheet, properties, rowIndex);
        }

        private static ExcelWorksheet CreateSheet(ExcelPackage excelPackage, SheetModel sheetModel)
        {
            return excelPackage.Workbook.Worksheets.Add(sheetModel.Title);
        }

        private static void WriteHeaders(ExcelWorksheet sheet, IEnumerable<PropertyInfo> properties, int rowIndex)
        {
            var propertyIndex = 1;
            foreach (var propertyInfo in properties)
            {
                sheet.Cells[rowIndex, propertyIndex].Value = propertyInfo.Name;

                propertyIndex++;
            }
        }

        private static void WriteRecords(IEnumerable records, ExcelWorksheet sheet, IEnumerable<PropertyInfo> properties, int rowIndex)
        {
            foreach (var record in records)
            {
                var propertyIndex = 1;
                foreach (var propertyInfo in properties)
                {
                    sheet.Cells[rowIndex, propertyIndex].Value = propertyInfo.GetValue(record, null);

                    propertyIndex++;
                }

                rowIndex++;
            }
        }
    }
}