using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Trading.Security.Chart
{
    public class ChartViewModel : Workspace
    {
        private IChartService _service;

        public ChartViewModel(ILog log, IDispatcherService dispatcherService, IChartService service)
            : base(log, dispatcherService)
        {
            _service = service;

            Disposables.Add(service);
        }
    }
}