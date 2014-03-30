using System;
using System.Collections.Generic;

using Blitz.Client.Common.ReportParameter;

using Common.Logging;

using Naru.WPF.Dialog;
using Naru.WPF.Scheduler;
using Naru.WPF.Wizard;

namespace Blitz.Client.Employee.ReportParameters
{
    public class ReportParameterViewModel : ReportParameterWizardViewModel<ReportParameterContext>
    {
        private readonly Func<ReportParameterStepViewModel> _reportParameterStepViewModelFactory;

        public ReportParameterViewModel(ILog log, IDispatcherSchedulerProvider scheduler, IStandardDialog standardDialog,
                                        Func<ReportParameterStepViewModel> reportParameterStepViewModelFactory)
            : base(log, scheduler, standardDialog)
        {
            _reportParameterStepViewModelFactory = reportParameterStepViewModelFactory;
        }

        protected override IEnumerable<IWizardStepViewModel<ReportParameterContext>> GetSteps()
        {
            yield return _reportParameterStepViewModelFactory();
        }
    }
}