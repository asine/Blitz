using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Blitz.Common.Core
{
    public interface ILog4NetConfiguration
    {
        string LogDirectoryPath { get; set; }
        string LogFileName { get; set; }
    }

    public class Log4NetConfiguration : ILog4NetConfiguration
    {
        private string _logDirectoryPath;
        private string _logFileName;
        private readonly string _sectionName;

        public Log4NetConfiguration(string sectionName)
        {
            _sectionName = sectionName;
        }

        public string LogDirectoryPath
        {
            get
            {
                if (string.IsNullOrEmpty(_logDirectoryPath))
                {
                    var section = (NameValueCollection)ConfigurationManager.GetSection(_sectionName);
                    if (section == null)
                    {
                        throw new ConfigurationErrorsException(string.Format("Missing section in application configuration file: {0}", _sectionName));
                    }
                    _logDirectoryPath = Environment.ExpandEnvironmentVariables(section["LogDirectoryPath"]);
                }
                return _logDirectoryPath;
            }
            set { _logDirectoryPath = value; }
        }

        public string LogFileName
        {
            get
            {
                if (string.IsNullOrEmpty(_logFileName))
                {
                    var section = (NameValueCollection)ConfigurationManager.GetSection(_sectionName);
                    if (section == null)
                    {
                        throw new ConfigurationErrorsException(string.Format("Missing section in application configuration file: {0}", _sectionName));
                    }
                    _logFileName = Environment.ExpandEnvironmentVariables(section["LogFileName"]);
                }
                return _logFileName;
            }
            set { _logFileName = value; }
        }
    }
}