using Common.Logging;

using Naru.WPF.MVVM;

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