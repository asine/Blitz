using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using OfficeOpenXml;

namespace Blitz.Client.Core.EPPlus
{
    public class ReflectionDataWriter
    {
        public void Write(ExcelWorksheet excelWorksheet, DataWriterModel dataWriterModel, IEnumerable<object> records)
        {
            if (!records.Any()) return;

            var properties = records.First().GetType().GetProperties();

            WriteHeaders(excelWorksheet, properties, dataWriterModel);

            WriteRecords(records, excelWorksheet, properties, dataWriterModel);
        }

        public void Write<T>(ExcelWorksheet excelWorksheet, DataWriterModel dataWriterModel, IEnumerable<T> records)
        {
            if (!records.Any()) return;

            var properties = typeof(T).GetProperties();

            WriteHeaders(excelWorksheet, properties, dataWriterModel);

            WriteRecords(records, excelWorksheet, properties, dataWriterModel);
        }

        private static void WriteHeaders(ExcelWorksheet excelWorksheet, IEnumerable<PropertyInfo> properties, DataWriterModel dataWriterModel)
        {
            foreach (var propertyInfo in properties)
            {
                excelWorksheet.Cells[dataWriterModel.CurrentRow, dataWriterModel.CurrentColumn].Value = propertyInfo.Name;

                dataWriterModel.NextColumn();
            }

            dataWriterModel.NextRow();
            dataWriterModel.RestartColumns();
        }

        private static void WriteRecords(IEnumerable records, ExcelWorksheet excelWorksheet, IEnumerable<PropertyInfo> properties, DataWriterModel dataWriterModel)
        {
            foreach (var record in records)
            {
                foreach (var propertyInfo in properties)
                {
                    excelWorksheet.Cells[dataWriterModel.CurrentRow, dataWriterModel.CurrentColumn].Value = propertyInfo.GetValue(record, null);

                    dataWriterModel.NextColumn();
                }

                dataWriterModel.NextRow();
                dataWriterModel.RestartColumns();
            }
        }
    }
}