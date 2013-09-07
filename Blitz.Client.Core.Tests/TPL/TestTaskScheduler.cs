using System.Threading.Tasks;

using Blitz.Client.Core.TPL;

namespace Blitz.Client.Core.Tests.TPL
{
    public class TestTaskScheduler : ITaskScheduler
    {
        public TaskScheduler Default { get; private set; }

        public TestTaskScheduler()
        {
            Default = new CurrentThreadTaskScheduler();
        }
    }
}