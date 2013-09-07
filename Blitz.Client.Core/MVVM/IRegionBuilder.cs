using System;
using System.Threading.Tasks;

namespace Blitz.Client.Core.MVVM
{
    public interface IRegionBuilder
    {
        void Clear(string regionName);

        Task ClearAsync(string regionName);
    }

    public interface IRegionBuilder<TViewModel>
            where TViewModel : IViewModel
    {
        IRegionBuilder<TViewModel> WithScope();

        IRegionBuilder<TViewModel> WithInitialisation(Action<TViewModel> initialiseViewModel);

        TViewModel Show(string regionName);

        Task<TViewModel> ShowAsync(string regionName);

        void Show(string regionName, TViewModel viewModel);

        Task ShowAsync(string regionName, TViewModel viewModel);
    }
}