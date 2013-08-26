using System;

namespace Blitz.Client.Core.Agatha
{
    public sealed class RequestException : Exception
    {
        public RequestException(string message)
            : base(message)
        { }
    }
}