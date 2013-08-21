using System;

namespace Blitz.Client.Core.MVVM
{
    public interface ISupportClosing
    {
        void Close();

        event EventHandler Closed;
    }
}