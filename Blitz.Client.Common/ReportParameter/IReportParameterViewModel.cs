using System;

using Naru.TPL;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Common.ReportParameter
{
    public interface IReportParameterViewModel : IViewModel, ISupportActivationState
    {
        IObservable<Unit> GenerateReport { get; } 
    }
}