using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

using Moq;

using NUnit.Framework;

namespace Blitz.Client.Core.Tests.MVVM
{
    [TestFixture]
    public class ViewModelHelpersTests
    {
        public class StubISupportActivationState : Workspace
        {
            public StubISupportActivationState(ILog log, IDispatcherService dispatcherService) 
                : base(log, dispatcherService)
            {
            }
        }

        [Test]
        public void when_parent_is_activated_child_is_activated()
        {
            var parent = new StubISupportActivationState(new Mock<ILog>().Object, new Mock<IDispatcherService>().Object) as ISupportActivationState;
            parent.DeActivate();
            Assert.That(parent.IsActive, Is.False);

            var child = new StubISupportActivationState(new Mock<ILog>().Object, new Mock<IDispatcherService>().Object) as ISupportActivationState;
            child.DeActivate();
            Assert.That(child.IsActive, Is.False);

            parent.SyncViewModelActivationStates(child);

            parent.Activate();

            Assert.That(parent.IsActive, Is.True);
            Assert.That(child.IsActive, Is.True);
        }

        [Test]
        public void when_parent_is_deactivated_child_is_activated()
        {
            var parent = new StubISupportActivationState(new Mock<ILog>().Object, new Mock<IDispatcherService>().Object) as ISupportActivationState;
            parent.Activate();
            Assert.That(parent.IsActive, Is.True);

            var child = new StubISupportActivationState(new Mock<ILog>().Object, new Mock<IDispatcherService>().Object) as ISupportActivationState;
            child.Activate();
            Assert.That(child.IsActive, Is.True);

            parent.SyncViewModelActivationStates(child);

            parent.DeActivate();

            Assert.That(parent.IsActive, Is.False);
            Assert.That(child.IsActive, Is.False);
        }
    }
}