using System;

namespace Blitz.Client.Core.MVVM
{
    public interface IRegionBuilder
    {
        void Clear(string regionName);
    }

    public interface IRegionBuilder<TViewModel>
            where TViewModel : IViewModel
    {
        IRegionBuilder<TViewModel> WithScope();

        IRegionBuilder<TViewModel> WithInitialisation(Action<TViewModel> initialiseViewModel);

        void Show(string regionName, TViewModel viewModel);

        TViewModel Show(string regionName);
    }
}