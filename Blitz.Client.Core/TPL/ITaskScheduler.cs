using System.Threading.Tasks;

namespace Blitz.Client.Core.TPL
{
    public interface ITaskScheduler
    {
        TaskScheduler Default { get; }
    }
}