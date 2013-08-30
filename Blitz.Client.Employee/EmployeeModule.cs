using Blitz.Client.Common;
using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Modularity;

namespace Blitz.Client.Employee
{
    public class EmployeeModule : IModule
    {
        private readonly ILog _log;
        private readonly IViewService _viewService;

        public EmployeeModule(ILog log, IViewService viewService)
        {
            _log = log;
            _viewService = viewService;
        }

        public void Initialize()
        {
            _log.Info("Adding Employee Report to Main region");
            _viewService.RegionBuilder<ReportViewModel>()
                .WithScope()
                .WithInitialisation(x => x.DisplayName = "Employee Report2")
                .Show(RegionNames.MAIN);
        }
    }
}