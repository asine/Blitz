using Common.Logging;

using Naru.WPF.ViewModel;

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