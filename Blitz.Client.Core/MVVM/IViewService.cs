using System;

namespace Blitz.Client.Core.MVVM
{
    public interface IViewService
    {
        void AddToRegion(IViewModel viewModel, string regionName, bool scoped = false);

        void AddToRegion<TViewModel>(string regionName, bool scoped = false)
            where TViewModel : IViewModel;

        void AddToRegion<TViewModel>(string regionName, Action<TViewModel> initialiseViewModel, bool scoped = false)
            where TViewModel : IViewModel;
    }
}