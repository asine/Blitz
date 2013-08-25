using System;

using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Customer.ReportLayout
{
    public class ReportLayoutItemViewModel : ViewModel
    {
        public Guid Id { get; private set; }

        public AttributeType Type { get; set; }

        public ReportLayoutItemViewModel(ILog log)
            : base(log)
        {
            Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Type, DisplayName);
        }
    }

    public enum AttributeType
    {
        Dimension,
        Measure
    }
}