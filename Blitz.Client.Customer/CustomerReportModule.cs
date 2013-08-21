using Blitz.Client.Common;
using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Modularity;

namespace Blitz.Client.Customer
{
    public class CustomerReportModule : IModule
    {
        private readonly ILog _log;
        private readonly IViewService _viewService;

        public CustomerReportModule(ILog log, IViewService viewService)
        {
            _log = log;
            _viewService = viewService;
        }

        public void Initialize()
        {
            _log.Info("Adding Customer Report1 to Main region");
            _viewService.RegionBuilder<CustomerReportViewModel>()
                .WithScope()
                .WithInitialisation(x => x.DisplayName = "Customer Report1")
                .Show(RegionNames.MAIN);

            _log.Info("Adding Customer Report2 to Main region");
            _viewService.RegionBuilder<CustomerReportViewModel>()
                .WithScope()
                .WithInitialisation(x => x.DisplayName = "Customer Report2")
                .Show(RegionNames.MAIN);
        }
    }
}