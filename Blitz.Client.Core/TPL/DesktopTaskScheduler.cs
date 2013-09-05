using System.Threading.Tasks;

namespace Blitz.Client.Core.TPL
{
    public class DesktopTaskScheduler : ITaskScheduler
    {
        public TaskScheduler Default { get; private set; }

        public DesktopTaskScheduler()
        {
            Default = TaskScheduler.Default;
        }
    }
}