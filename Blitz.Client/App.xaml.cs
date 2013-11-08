using System.Windows;

namespace Blitz.Client
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Naru.Core.UnhandledExceptionHandler.InstallDomainUnhandledException();
            Naru.TPL.UnhandledExceptionHandler.InstallTaskUnobservedException();
            Naru.WPF.UnhandledExceptionHandler.InstallDispatcherUnhandledException();

            new Bootstrapper();
        }
    }
}