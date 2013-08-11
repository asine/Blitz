using System;

using Microsoft.Practices.Prism.Events;

namespace Blitz.Client.Core.MVVM
{
    public static class ViewModelHelpers
    {
        public static IDisposable SyncViewModelActivationStates(this ISupportActivationState source,
            ISupportActivationState viewModel)
        {
            EventHandler<DataEventArgs<bool>> sourceOnActivationStateChanged = (s, e) =>
            {
                if (e.Value)
                    viewModel.Activate();
                else
                    viewModel.DeActivate();
            };
            source.ActivationStateChanged += sourceOnActivationStateChanged;

            return new AnonymousDisposable(() => source.ActivationStateChanged -= sourceOnActivationStateChanged);
        }
    }
}