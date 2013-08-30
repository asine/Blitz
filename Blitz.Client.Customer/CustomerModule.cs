using Blitz.Client.Common;
using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Modularity;

namespace Blitz.Client.Customer
{
    public class CustomerModule : IModule
    {
        private readonly ILog _log;
        private readonly IViewService _viewService;

        public CustomerModule(ILog log, IViewService viewService)
        {
            _log = log;
            _viewService = viewService;
        }

        public void Initialize()
        {
            _log.Info("Adding Customer Report to Main region");
            _viewService.RegionBuilder<ReportViewModel>()
                .WithScope()
                .WithInitialisation(x => x.DisplayName = "Customer Report1")
                .Show(RegionNames.MAIN);
        }
    }
}