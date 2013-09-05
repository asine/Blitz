using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blitz.Client.Core.Tests.TPL
{
    public class CurrentThreadTaskScheduler : TaskScheduler
    {
        // http://www.kjetilk.com/2012/11/unit-testing-asynchronous-operations.html

        protected override void QueueTask(Task task)
        {
            TryExecuteTask(task);
        }

        protected override bool TryExecuteTaskInline(
           Task task,
           bool taskWasPreviouslyQueued)
        {
            return TryExecuteTask(task);
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return Enumerable.Empty<Task>();
        }

        public override int MaximumConcurrencyLevel { get { return 1; } }
    }
}