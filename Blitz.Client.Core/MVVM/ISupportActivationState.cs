using System;

using Microsoft.Practices.Prism.Events;

namespace Blitz.Client.Core.MVVM
{
    public interface ISupportActivationState
    {
        bool IsActive { get; }

        void Activate();

        void DeActivate();

        event EventHandler<DataEventArgs<bool>>  ActivationStateChanged;
    }
}