using System;

namespace Blitz.Client.Core.MVVM
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class UseViewAttribute : Attribute
    {
        public Type ViewType { get; private set; }

        public UseViewAttribute(Type viewType)
        {
            ViewType = viewType;
        }
    }
}