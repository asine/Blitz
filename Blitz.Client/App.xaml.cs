using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace Blitz.Client
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Debugger.Break();
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Debugger.Break();
        }
    }
}