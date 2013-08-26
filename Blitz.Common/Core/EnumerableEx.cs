using System.Collections.Generic;

namespace Blitz.Common.Core
{
    public static class EnumerableEx
    {
        public static IEnumerable<T> Return<T>(T value)
        {
            yield return value;
        }
    }
}