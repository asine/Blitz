using System;
using System.Collections.Generic;

using Blitz.Client.Common.ReportParameter;

using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.Wizard;

namespace Blitz.Client.Customer.ReportParameters
{
    [UseView(typeof(WizardView))]
    public class ReportParameterViewModel : ReportParameterWizardViewModel<ReportParameterContext>
    {
        private readonly Func<ReportParameterStepViewModel> _reportParameterStepViewModelFactory;

        public ReportParameterViewModel(ILog log, ISchedulerProvider scheduler, IViewService viewService,
                                        Func<ReportParameterStepViewModel> reportParameterStepViewModelFactory)
            : base(log, scheduler, viewService)
        {
            _reportParameterStepViewModelFactory = reportParameterStepViewModelFactory;
        }

        protected override IEnumerable<IWizardStepViewModel<ReportParameterContext>> GetSteps()
        {
            yield return _reportParameterStepViewModelFactory();
            yield return _reportParameterStepViewModelFactory();
            yield return _reportParameterStepViewModelFactory();
            yield return _reportParameterStepViewModelFactory();
            yield return _reportParameterStepViewModelFactory();
        }
    }
}