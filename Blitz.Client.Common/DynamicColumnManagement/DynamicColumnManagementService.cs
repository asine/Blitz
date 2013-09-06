using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Common.DynamicColumnManagement
{
    public interface IDynamicColumnManagementService : IService
    {
    }

    public class DynamicColumnManagementService : Service, IDynamicColumnManagementService
    {
        public DynamicColumnManagementService(ILog log)
            : base(log)
        {
        }
    }
}