using Blitz.Client.Core.MVVM.Dialog;

namespace Blitz.Client.Core.MVVM
{
    public interface IViewService
    {
        IRegionBuilder<TViewModel> RegionBuilder<TViewModel>()
            where TViewModel : IViewModel;

        IRegionBuilder RegionBuilder();

        IDialogBuilder DialogBuilder();

        void ShowModel(IViewModel viewModel);
    }
}