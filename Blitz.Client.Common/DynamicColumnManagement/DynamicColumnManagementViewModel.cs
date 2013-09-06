using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Common.DynamicColumnManagement
{
    public class DynamicColumnManagementViewModel : Workspace
    {
        private IDynamicColumnManagementService _service;

        public DynamicColumnManagementViewModel(ILog log, IDispatcherService dispatcherService, IDynamicColumnManagementService service)
            : base(log, dispatcherService)
        {
            _service = service;

            Disposables.Add(service);
        }
    }
}