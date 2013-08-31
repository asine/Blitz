using Blitz.Client.Core.MVVM;
using Blitz.Common.Core;

using Microsoft.Practices.Prism.Modularity;

namespace Blitz.Client.Blotter
{
    public class BlotterModule : IModule
    {
        private readonly ILog _log;
        private readonly IViewService _viewService;

        public BlotterModule(ILog log, IViewService viewService)
        {
            _log = log;
            _viewService = viewService;
        }

        public void Initialize()
        {
        }
    }
}