using System;
using System.Collections.Generic;

namespace Blitz.Client.Core.MVVM
{
    /// <summary>
    /// Represents a group of disposable resources that are disposed together.
    /// </summary>
    public class CompositeDisposable : IDisposable
    {
        private readonly List<IDisposable> _disposables;
        private readonly object _gate = new object();

        private bool _disposed;

        public CompositeDisposable()
        {
            _disposables = new List<IDisposable>();
        }

        /// <summary>
        /// Adds a disposable to the CompositeDisposable or disposes the disposable if the CompositeDisposable is disposed.
        /// </summary>
        /// <param name="item">Disposable to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
        public void Add(IDisposable item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            var shouldDispose = false;
            lock (_gate)
            {
                shouldDispose = _disposed;
                if (!_disposed)
                {
                    _disposables.Add(item);
                }
            }

            if (shouldDispose)
            {
                item.Dispose();
            }
        }

        /// <summary>
        /// Removes and disposes all disposables from the CompositeDisposable, but does not dispose the CompositeDisposable.
        /// </summary>
        public void Clear()
        {
            var currentDisposables = default(IDisposable[]);
            lock (_gate)
            {
                currentDisposables = _disposables.ToArray();
                _disposables.Clear();
            }

            if (currentDisposables == null)
            {
                return;
            }

            foreach (var d in currentDisposables)
            {
                if (d == null) continue;
                d.Dispose();
            }
        }

        /// <summary>
        /// Disposes all disposables in the group and removes them from the group.
        /// </summary>
        public void Dispose()
        {
            var currentDisposables = default(IDisposable[]);
            lock (_gate)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    currentDisposables = _disposables.ToArray();
                    _disposables.Clear();
                }
            }

            if (currentDisposables == null)
            {
                return;
            }

            foreach (var d in currentDisposables)
            {
                if (d == null) continue;

                d.Dispose();
            }
        }
    }
}