using System;

using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;
using Blitz.Tests.UnityAutoMockContainer;

using NUnit.Framework;

namespace Blitz.Client.Core.Tests.MVVM
{
    [TestFixture]
    public class WorkspaceTests
    {
        public class WorkspaceViewModel : Workspace
        {
            public WorkspaceViewModel(ILog log, IDispatcherService dispatcherService) 
                : base(log, dispatcherService)
            {
                Disposables.Add(AnonymousDisposable.Create(() => DisposablesWasDisposed.SafeInvoke(this)));
            }

            public event EventHandler CleanUpWasCalled;

            public event EventHandler DisposablesWasDisposed;

            public event EventHandler OnInitialiseWasCalled;

            public event EventHandler OnActivateWasCalled;

            public event EventHandler OnDeActivateWasCalled;

            protected override void OnInitialise()
            {
                OnInitialiseWasCalled.SafeInvoke(this);
            }

            protected override void OnActivate()
            {
                OnActivateWasCalled.SafeInvoke(this);
            }

            protected override void OnDeActivate()
            {
                OnDeActivateWasCalled.SafeInvoke(this);
            }

            protected override void CleanUp()
            {
                CleanUpWasCalled.SafeInvoke(this);
            }

            public new void Busy(string message)
            {
                base.Busy(message);
            }

            public new void Idle()
            {
                base.Idle();
            }
        }

        [TestFixture]
        public class ISupportClosing
        {
            public class when_Close
            {
                [Test]
                public void is_called_then_OnClosed_event_is_fired()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = false;

                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.False);

                    viewModel.OnClosed += (s, e) => eventWasFired = true;

                    viewModel.Close();

                    Assert.That(eventWasFired, Is.True);
                }

                [Test]
                public void is_called_then_Disposables_are_disposed()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = false;

                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.False);

                    viewModel.DisposablesWasDisposed += (s, e) => eventWasFired = true;

                    viewModel.Close();

                    Assert.That(eventWasFired, Is.True);
                }

                [Test]
                public void is_called_then_CleanUp_is_called()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = false;

                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.False);

                    viewModel.CleanUpWasCalled += (s, e) => eventWasFired = true;

                    viewModel.Close();

                    Assert.That(eventWasFired, Is.True);
                }
            }

            [Test]
            public void when_CloseCommand_is_executed_then_OnClosed_event_is_fired()
            {
                var container = new UnityAutoMockContainer();

                var eventWasFired = false;

                var viewModel = container.Resolve<WorkspaceViewModel>();

                Assert.That(eventWasFired, Is.False);

                viewModel.OnClosed += (s, e) => eventWasFired = true;

                viewModel.ClosingCommand.Execute();

                Assert.That(eventWasFired, Is.True);
            }
        }

        public class ISupportActivationStateTests
        {
            [TestFixture]
            public class when_Activate
            {
                [Test]
                public void is_called_multiple_time_then_OnInitialise_is_only_called_once()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = 0;

                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.EqualTo(0));

                    viewModel.OnInitialiseWasCalled += (s, e) => eventWasFired++;

                    ((ISupportActivationState)viewModel).Activate();
                    ((ISupportActivationState)viewModel).DeActivate();
                    ((ISupportActivationState)viewModel).Activate();

                    Assert.That(eventWasFired, Is.EqualTo(1));
                }

                [Test]
                public void is_called_multiple_time_then_OnInitialised_event_is_only_fired_once()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = 0;

                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.EqualTo(0));

                    viewModel.OnInitialised += (s, e) => eventWasFired++;

                    ((ISupportActivationState)viewModel).Activate();
                    ((ISupportActivationState)viewModel).DeActivate();
                    ((ISupportActivationState)viewModel).Activate();

                    Assert.That(eventWasFired, Is.EqualTo(1));
                }

                [Test]
                public void is_called_then_IsActive_is_true()
                {
                    var container = new UnityAutoMockContainer();

                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(viewModel.IsActive, Is.False);

                    ((ISupportActivationState)viewModel).Activate();

                    Assert.That(viewModel.IsActive, Is.True);
                }

                [Test]
                public void is_called_then_OnActivationStateChanged_event_is_fired()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = false;

                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.False);

                    viewModel.OnActivationStateChanged += (s, e) => eventWasFired = true;

                    ((ISupportActivationState)viewModel).Activate();

                    Assert.That(eventWasFired, Is.True);
                }

                [Test]
                public void is_called_then_OnActivate_is_called()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = false;

                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.False);

                    viewModel.OnActivateWasCalled += (s, e) => eventWasFired = true;

                    ((ISupportActivationState)viewModel).Activate();

                    Assert.That(eventWasFired, Is.True);
                }
            }

            [TestFixture]
            public class when_DeActivate
            {
                [Test]
                public void is_called_then_IsActive_is_false()
                {
                    var container = new UnityAutoMockContainer();

                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(viewModel.IsActive, Is.False);

                    ((ISupportActivationState)viewModel).Activate();

                    Assert.That(viewModel.IsActive, Is.True);

                    ((ISupportActivationState)viewModel).DeActivate();

                    Assert.That(viewModel.IsActive, Is.False);
                }

                [Test]
                public void is_called_then_OnActivationStateChanged_event_is_fired()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = false;

                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.False);

                    viewModel.OnActivationStateChanged += (s, e) => eventWasFired = true;

                    ((ISupportActivationState)viewModel).DeActivate();

                    Assert.That(eventWasFired, Is.True);
                }

                [Test]
                public void is_called_then_OnDeActivate_is_called()
                {
                    var container = new UnityAutoMockContainer();

                    var eventWasFired = false;

                    var viewModel = container.Resolve<WorkspaceViewModel>();

                    Assert.That(eventWasFired, Is.False);

                    viewModel.OnDeActivateWasCalled += (s, e) => eventWasFired = true;

                    ((ISupportActivationState)viewModel).DeActivate();

                    Assert.That(eventWasFired, Is.True);
                }
            }
        }

        public class when_Busy
        {
            [Test]
            public void is_called_then_IsBusy_is_set_to_true()
            {
                var container = new UnityAutoMockContainer();

                var viewModel = container.Resolve<WorkspaceViewModel>();

                Assert.That(viewModel.IsBusy, Is.False);

                viewModel.Busy(string.Empty);

                Assert.That(viewModel.IsBusy, Is.True);
            }

            [Test]
            public void is_called_with_a_message_then_BusyMessage_is_set_to_the_message()
            {
                var container = new UnityAutoMockContainer();

                var viewModel = container.Resolve<WorkspaceViewModel>();

                Assert.That(viewModel.BusyMessage, Is.Null);

                var busyMessage = Guid.NewGuid().ToString();

                viewModel.Busy(busyMessage);

                Assert.That(viewModel.BusyMessage, Is.EqualTo(busyMessage));
            }
        }

        public class when_Idle
        {
            [Test]
            public void is_called_then_IsBusy_is_set_to_false()
            {
                var container = new UnityAutoMockContainer();

                var viewModel = container.Resolve<WorkspaceViewModel>();
                viewModel.Busy(string.Empty);

                Assert.That(viewModel.IsBusy, Is.True);

                viewModel.Idle();

                Assert.That(viewModel.IsBusy, Is.False);
            }

            [Test]
            public void is_called_then_BusyMessage_is_set_to_empty_string()
            {
                var container = new UnityAutoMockContainer();

                var viewModel = container.Resolve<WorkspaceViewModel>();

                var busyMessage = Guid.NewGuid().ToString();

                viewModel.Busy(busyMessage);

                viewModel.Idle();

                Assert.That(viewModel.BusyMessage, Is.EqualTo(string.Empty));
            }
        }
    }
}