using System;
using System.Threading;

namespace Blitz.Client.Core.MVVM
{
    public sealed class AnonymousDisposable : IDisposable
    {
        // http://stackoverflow.com/questions/7409759/how-to-store-actions-that-do-not-prevent-garbage-collection-of-variables-they-us

        private readonly Action _dispose;
        private int _isDisposed;

        public AnonymousDisposable(Action dispose)
        {
            _dispose = dispose;
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _isDisposed, 1) == 0)
            {
                _dispose();
            }
        }

        public static AnonymousDisposable Create(Action action)
        {
            return new AnonymousDisposable(action);
        }
    }
}