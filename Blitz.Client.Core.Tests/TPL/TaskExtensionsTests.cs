using System;
using System.Threading;
using System.Threading.Tasks;

using Blitz.Client.Core.TPL;

using NUnit.Framework;

namespace Blitz.Client.Core.Tests.TPL
{
    [TestFixture]
    public class TaskExtensionsTests
    {
        [Test]
        public void catch_happens_before_finally_with_TaskScheduler()
        {
            var autoResetEvent = new AutoResetEvent(false);

            var task1HasRun = false;
            var task2HasRun = false;
            var task3HasRun = false;

            var scheduler = TaskScheduler.Default;

            new TaskFactory(scheduler)
                .StartNew(() =>
                {
                    Assert.That(task1HasRun, Is.False);
                    Assert.That(task2HasRun, Is.False);
                    Assert.That(task3HasRun, Is.False);
                    task1HasRun = true;

                    throw new Exception();
                })
                .CatchAndHandle(_ =>
                {
                    Assert.That(task1HasRun, Is.True);
                    Assert.That(task2HasRun, Is.False);
                    Assert.That(task3HasRun, Is.False);
                    task2HasRun = true;
                }, scheduler)
                .Finally(() =>
                {
                    Assert.That(task1HasRun, Is.True);
                    Assert.That(task2HasRun, Is.True);
                    Assert.That(task3HasRun, Is.False);
                    task3HasRun = true;

                    autoResetEvent.Set();
                }, scheduler);

            autoResetEvent.WaitOne();

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.True);
            Assert.That(task3HasRun, Is.True);
        }

        [Test]
        public void catch_happens_before_finally_with_CurrentThreadScheduler()
        {
            var task1HasRun = false;
            var task2HasRun = false;
            var task3HasRun = false;

            var scheduler = new CurrentThreadTaskScheduler();

            new TaskFactory(scheduler)
                .StartNew(() =>
                {
                    Assert.That(task1HasRun, Is.False);
                    Assert.That(task2HasRun, Is.False);
                    Assert.That(task3HasRun, Is.False);
                    task1HasRun = true;

                    throw new Exception();
                })
                .CatchAndHandle(_ =>
                {
                    Assert.That(task1HasRun, Is.True);
                    Assert.That(task2HasRun, Is.False);
                    Assert.That(task3HasRun, Is.False);
                    task2HasRun = true;
                }, scheduler)
                .Finally(() =>
                {
                    Assert.That(task1HasRun, Is.True);
                    Assert.That(task2HasRun, Is.True);
                    Assert.That(task3HasRun, Is.False);
                    task3HasRun = true;
                }, scheduler);

            Assert.That(task1HasRun, Is.True);
            Assert.That(task2HasRun, Is.True);
            Assert.That(task3HasRun, Is.True);
        }
    }
}