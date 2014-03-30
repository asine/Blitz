using Common.Logging;

using Naru.WPF.Dialog;
using Naru.WPF.Scheduler;
using Naru.WPF.Wizard;

namespace Blitz.Client.Common.ReportParameter
{
    public abstract class ReportParameterWizardStepViewModel<TContext> : WizardStepViewModel<TContext>
        where TContext : IWizardContext, new()
    {
        protected ReportParameterWizardStepViewModel(ILog log, IDispatcherSchedulerProvider scheduler, IStandardDialog standardDialog) 
            : base(log, scheduler, standardDialog)
        {
        }
    }
}