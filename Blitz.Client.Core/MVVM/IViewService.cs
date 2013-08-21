using System;

using Blitz.Client.Core.MVVM.Dialog;

namespace Blitz.Client.Core.MVVM
{
    public interface IViewService
    {
        void AddToRegion(IViewModel viewModel, string regionName, bool scoped = false);

        void AddToRegion<TViewModel>(string regionName, bool scoped = false)
            where TViewModel : IViewModel;

        void AddToRegion<TViewModel>(string regionName, Action<TViewModel> initialiseViewModel, bool scoped = false)
            where TViewModel : IViewModel;

        void ClearRegion(string regionName);

        IDialogBuilder DialogBuilder();

        void ShowModel(IViewModel viewModel);
    }
}