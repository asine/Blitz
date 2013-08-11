using System;

using Microsoft.Practices.Prism.Events;

namespace Blitz.Client.Core.MVVM
{
    public interface ISupportActivationState
    {
        void Activate();
        void DeActivate();

        event EventHandler<DataEventArgs<bool>>  ActivationStateChanged;
    }
}