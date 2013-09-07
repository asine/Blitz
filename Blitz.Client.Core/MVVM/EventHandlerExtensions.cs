using System;

namespace Blitz.Client.Core.MVVM
{
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Safely invoke an event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventHandler"></param>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public static void SafeInvoke<T>(this EventHandler<T> eventHandler, object sender, T eventArgs) 
            where T : EventArgs
        {
            if (eventHandler != null)
            {
                eventHandler(sender, eventArgs);
            }
        }

        /// <summary>
        /// Safely invoke an event.
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <param name="sender"></param>
        public static void SafeInvoke(this EventHandler eventHandler, object sender)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, EventArgs.Empty);
            }
        }
    }
}