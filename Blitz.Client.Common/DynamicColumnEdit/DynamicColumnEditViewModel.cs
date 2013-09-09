using Blitz.Client.Common.DynamicReportData;

using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.TPL;

namespace Blitz.Client.Common.DynamicColumnEdit
{
    public class DynamicColumnEditViewModel : Workspace
    {
        private IDynamicColumnEditService _service;

        #region Header

        private string _header;

        public string Header
        {
            get { return _header; }
            set
            {
                if (value == _header) return;
                _header = value;
                RaisePropertyChanged(() => Header);
            }
        }

        #endregion

        #region IsVisible

        private bool _isVisible;

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value.Equals(_isVisible)) return;
                _isVisible = value;
                RaisePropertyChanged(() => IsVisible);
            }
        }

        #endregion

        public DynamicColumnEditViewModel(ILog log, IScheduler scheduler, IDynamicColumnEditService service)
            : base(log, scheduler)
        {
            _service = service;

            Disposables.Add(service);
        }

        public void Initialise(DynamicColumn column)
        {
            Header = column.HeaderName;
            IsVisible = column.IsVisible;
        }
    }
}