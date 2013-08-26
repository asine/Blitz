using System.IO;

using Blitz.Common.Core;

using OfficeOpenXml;

namespace Blitz.Client.Core.EPPlus
{
    public class PackageWriter
    {
        private readonly ILog _log;

        public PackageWriter(ILog log)
        {
            _log = log;
        }

        public ExcelPackage Create(PackageModel packageModel)
        {
            ExcelPackage excelPackage;

            if (string.IsNullOrEmpty(packageModel.TemplatePath))
            {
                _log.Info(string.Format("Creating ExcelPackage"));
                excelPackage = new ExcelPackage();
            }
            else
            {
                _log.Info(string.Format("Creating ExcelPackage using template {0}", packageModel.TemplatePath));
                var fileInfo = new FileInfo(packageModel.TemplatePath);
                excelPackage = new ExcelPackage(fileInfo);
            }

            return excelPackage;
        }

        public void Save(ExcelPackage excelPackage, PackageModel packageModel)
        {
            _log.Info(string.Format("Saving ExcelPackage to {0}", packageModel.SaveFilePath));

            var fileInfo = File.Create(packageModel.SaveFilePath);

            excelPackage.SaveAs(fileInfo);
        }
    }
}