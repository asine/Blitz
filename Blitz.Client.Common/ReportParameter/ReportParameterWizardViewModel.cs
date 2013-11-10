using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.Wizard;

namespace Blitz.Client.Common.ReportParameter
{
    public abstract class ReportParameterWizardViewModel<TContext> : WizardViewModel<TContext>, IReportParameterViewModel
        where TContext : IWizardContext, new()
    {
        private readonly Subject<Unit> _generateReportSubject = new Subject<Unit>();

        public IObservable<Unit> GenerateReport { get { return _generateReportSubject.AsObservable(); } }

        protected ReportParameterWizardViewModel(ILog log, ISchedulerProvider scheduler, IViewService viewService) 
            : base(log, scheduler, viewService)
        {
        }

        protected override void Finish()
        {
            _generateReportSubject.OnNext(Unit.Default);
            _generateReportSubject.OnCompleted();
        }
    }
}