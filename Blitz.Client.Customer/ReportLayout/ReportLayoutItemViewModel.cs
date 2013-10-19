using System;

using Common.Logging;

using Naru.WPF.ViewModel;

namespace Blitz.Client.Customer.ReportLayout
{
    public class ReportLayoutItemViewModel : ViewModel
    {
        public Guid Id { get; private set; }

        public string Name { get; set; }

        public AttributeType Type { get; set; }

        public ReportLayoutItemViewModel(ILog log)
            : base(log)
        {
            Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Type, Name);
        }
    }

    public enum AttributeType
    {
        Dimension,
        Measure
    }
}