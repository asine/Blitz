using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

namespace Blitz.Client.Customer.ReportLayout
{
    public class ReportLayoutItemViewModel : ViewModel
    {
        public ReportLayoutItemViewModel(ILog log)
            : base(log)
        {
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}