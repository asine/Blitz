using System.IO;

using Blitz.Common.Core;

using Common.Logging;

using OfficeOpenXml;

namespace Blitz.Client.Core.EPPlus
{
    public class ExcelPackageWriter
    {
        private readonly ILog _log;

        public ExcelPackageWriter(ILog log)
        {
            _log = log;
        }

        public ExcelPackage Create(ExcelPackageModel excelPackageModel)
        {
            ExcelPackage excelPackage;

            if (string.IsNullOrEmpty(excelPackageModel.TemplatePath))
            {
                _log.Debug(string.Format("Creating ExcelPackage"));
                excelPackage = new ExcelPackage();
            }
            else
            {
                _log.Debug(string.Format("Creating ExcelPackage using template {0}", excelPackageModel.TemplatePath));
                var fileInfo = new FileInfo(excelPackageModel.TemplatePath);
                excelPackage = new ExcelPackage(fileInfo);
            }

            return excelPackage;
        }

        public void Save(ExcelPackage excelPackage, ExcelPackageModel excelPackageModel)
        {
            _log.Debug(string.Format("Saving ExcelPackage to {0}", excelPackageModel.SaveFilePath));

            var fileInfo = File.Create(excelPackageModel.SaveFilePath);

            excelPackage.SaveAs(fileInfo);
        }
    }
}