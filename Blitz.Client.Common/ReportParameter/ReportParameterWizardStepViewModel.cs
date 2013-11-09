using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.Wizard;

namespace Blitz.Client.Common.ReportParameter
{
    public abstract class ReportParameterWizardStepViewModel<TContext> : WizardStepViewModel<TContext>
        where TContext : IWizardContext, new()
    {
        protected ReportParameterWizardStepViewModel(ILog log, ISchedulerProvider scheduler, IViewService viewService) 
            : base(log, scheduler, viewService)
        {
        }
    }
}