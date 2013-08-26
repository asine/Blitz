using System;
using System.Collections.Generic;

using Blitz.Client.Core.EPPlus;

namespace Blitz.Client.Common.ExportToExcel
{
    public class BasicExportToExcel : IBasicExportToExcel
    {
        private readonly ExcelPackageWriter _excelPackageWriter;
        private readonly ExcelWorkSheetWriter _excelWorkSheetWriter;
        private readonly ReflectionDataWriter _reflectionDataWriter;

        public BasicExportToExcel(ExcelPackageWriter excelPackageWriter,
            ExcelWorkSheetWriter excelWorkSheetWriter,
            ReflectionDataWriter reflectionDataWriter)
        {
            _excelPackageWriter = excelPackageWriter;
            _excelWorkSheetWriter = excelWorkSheetWriter;
            _reflectionDataWriter = reflectionDataWriter;
        }

        public void ExportToExcel(IEnumerable<IEnumerable<object>> reports)
        {
            var packageModel = new ExcelPackageModel { SaveFilePath = string.Format("{0}.xlsx", Guid.NewGuid()) };

            var excelPackage = _excelPackageWriter.Create(packageModel);

            var sheetId = 0;
            foreach (var records in reports)
            {
                var excelWorksheet = _excelWorkSheetWriter.Create(excelPackage, new ExcelWorkSheetModel {Title = string.Format("Report{0}", sheetId)});

                _reflectionDataWriter.Write(excelWorksheet, DataWriterModel.Default, records);

                sheetId++;
            }

            _excelPackageWriter.Save(excelPackage, packageModel);
        }
    }
}