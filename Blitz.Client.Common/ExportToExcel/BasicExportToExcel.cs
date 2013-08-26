using System;
using System.Collections.Generic;

using Blitz.Client.Core.EPPlus;

namespace Blitz.Client.Common.ExportToExcel
{
    public class BasicExportToExcel : IBasicExportToExcel
    {
        private readonly PackageWriter _packageWriter;
        private readonly ReflectionSheetWriter _reflectionSheetWriter;

        public BasicExportToExcel(PackageWriter packageWriter,
            ReflectionSheetWriter reflectionSheetWriter)
        {
            _packageWriter = packageWriter;
            _reflectionSheetWriter = reflectionSheetWriter;
        }

        public void ExportToExcel(IEnumerable<IEnumerable<object>> reports)
        {
            var packageModel = new PackageModel { SaveFilePath = string.Format("{0}.xlsx", Guid.NewGuid()) };

            var excelPackage = _packageWriter.Create(packageModel);

            var sheetId = 0;
            foreach (var records in reports)
            {
                _reflectionSheetWriter.Write(excelPackage,
                    new SheetModel { Title = string.Format("Report{0}", sheetId) },
                    records);

                sheetId++;
            }

            _packageWriter.Save(excelPackage, packageModel);
        }
    }
}