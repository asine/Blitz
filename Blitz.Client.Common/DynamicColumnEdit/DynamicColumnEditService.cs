using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Common.DynamicColumnEdit
{
    public interface IDynamicColumnEditService : IService
    {
    }

    public class DynamicColumnEditService : Service, IDynamicColumnEditService
    {
        public DynamicColumnEditService(ILog log)
            : base(log)
        {
        }
    }
}