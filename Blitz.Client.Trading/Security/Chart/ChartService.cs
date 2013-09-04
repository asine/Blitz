using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Trading.Security.Chart
{
    public interface IChartService : IService
    {
    }

    public class ChartService : Service, IChartService
    {
        public ChartService(ILog log)
            : base(log)
        {
        }
    }
}