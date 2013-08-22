using System;

namespace Blitz.Client.Core.MVVM
{
    public static class EventHandlerExtensions
    {
        public static void SafeInvoke<T>(this EventHandler<T> eventHandler, object sender, T eventArgs) 
            where T : EventArgs
        {
            if (eventHandler != null)
            {
                eventHandler(sender, eventArgs);
            }
        }

        public static void SafeInvoke(this EventHandler eventHandler, object sender)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, EventArgs.Empty);
            }
        }
    }
}