using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Common.DynamicColumnEdit
{
    public class DynamicColumnEditViewModel : Workspace
    {
        private IDynamicColumnEditService _service;

        public DynamicColumnEditViewModel(ILog log, IDispatcherService dispatcherService, IDynamicColumnEditService service)
            : base(log, dispatcherService)
        {
            _service = service;

            Disposables.Add(service);
        }
    }
}