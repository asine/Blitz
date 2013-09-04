using Blitz.Common.Core;

namespace Blitz.Client.Core.MVVM
{
    public abstract class Service : IService
    {
        protected readonly ILog Log;

        protected Service(ILog log)
        {
            Log = log;
        }

        public virtual void Dispose()
        { }
    }
}