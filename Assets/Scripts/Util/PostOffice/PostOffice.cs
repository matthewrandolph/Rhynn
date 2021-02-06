using System;
using System.Collections.Generic;

namespace Util.PostOffice
{
    /// <summary>
    /// An observer pattern class to communicate information between disparate domains. Be sure to define the message
    /// names within the Message enum. This class handles messages with no extra data attached to them.
    /// </summary>
    public class PostOffice
    {
        /// <summary>
        /// Add an event handler listening for a specific message that needs no data attached. Be sure to remove the
        /// event handler when it is no longer needed.
        /// </summary>
        /// <param name="eventType">The event to listen for.</param>
        /// <param name="handler">The method/function to call when the event is raised.</param>
        public static void AddObserver(Message eventType, Action handler)
        {
            if (!_messageTable.TryGetValue(eventType, out List<Action> handlers))
            {
                handlers = new List<Action>();
                _messageTable.Add(eventType, handlers);
            }

            if (!handlers.Contains(handler))
            {
                _messageTable[eventType].Add(handler);
            }
        }

        /// <summary>
        /// Removes an event handler that was previously listening for a specific message that needed no data attached.
        /// </summary>
        /// <param name="eventType">The event to listen for.</param>
        /// <param name="handler">The method/function to call when the event is raised.</param>
        public static void RemoveObserver(Message eventType, Action handler)
        {
            if (!_messageTable.TryGetValue(eventType, out List<Action> handlers)) return;

            if (handlers.Contains(handler))
            {
                _messageTable[eventType].Remove(handler);
            }
        }

        /// <summary>
        /// Raises an event that calls all the listeners assigned to a specific no-data-included message. Gracefully
        /// handles situations where there are no observers.
        /// </summary>
        /// <param name="eventType">The event to raise.</param>
        public static void SendEvent(Message eventType)
        {
            if (!_messageTable.TryGetValue(eventType, out List<Action> handlers)) return;

            foreach (Action handler in handlers)
            {
                handler();
            }
        }
        
        private static Dictionary<Message, List<Action>> _messageTable = new Dictionary<Message, List<Action>>();
    }
    
    /// <summary>
    /// An observer pattern class to communicate information between disparate domains. Be sure to define the message
    /// names within the Message enum. This class handles messages with a single unit of data attached to it.
    /// </summary>
    public class PostOffice<T>
    {
        /// <summary>
        /// Add an event handler listening for a specific message that has 1 unit of data attached. Be sure to remove
        /// the event handler when it is no longer needed.
        /// </summary>
        /// <param name="eventType">The event to listen for.</param>
        /// <param name="handler">The method/function to call when the event is raised.</param>
        public static void AddObserver(Message eventType, Action<T> handler)
        {
            if (!_messageTable.TryGetValue(eventType, out List<Action<T>> handlers))
            {
                handlers = new List<Action<T>>();
                _messageTable.Add(eventType, handlers);
            }

            if (!handlers.Contains(handler))
            {
                _messageTable[eventType].Add(handler);
            }
        }

        /// <summary>
        /// Removes an event handler that was previously listening for a specific message that had 1 unit of data attached.
        /// </summary>
        /// <param name="eventType">The event to listen for.</param>
        /// <param name="handler">The method/function to call when the event is raised.</param>
        public static void RemoveObserver(Message eventType, Action<T> handler)
        {
            if (!_messageTable.TryGetValue(eventType, out List<Action<T>> handlers)) return;

            if (handlers.Contains(handler))
            {
                _messageTable[eventType].Remove(handler);
            }
        }

        /// <summary>
        /// Raises an event that calls all the listeners assigned to a specific message that includes 1 unit of data.
        /// Gracefully handles situations where there are no observers.
        /// </summary>
        /// <param name="eventType">The event to raise.</param>
        /// <param name="parameter">The data to send with the message</param>
        public static void SendEvent(Message eventType, T parameter)
        {
            if (!_messageTable.TryGetValue(eventType, out List<Action<T>> handlers)) return;

            foreach (Action<T> handler in handlers)
            {
                handler(parameter);
            }
        }
        
        private static Dictionary<Message, List<Action<T>>> _messageTable = new Dictionary<Message, List<Action<T>>>();
    }    
    
    /// <summary>
    /// An observer pattern class to communicate information between disparate domains. Be sure to define the message
    /// names within the Message enum. This class handles messages with two units of data attached to it. For anything
    /// more complicated than sending a single extra value for 'U', create a class that inherits from MessageArgs to
    /// pass into the message.
    /// </summary>
    public class PostOffice<T, U>
    {
        /// <summary>
        /// Add an event handler listening for a specific message that has 2 units of data attached. Be sure to remove
        /// the event handler when it is no longer needed.
        /// </summary>
        /// <param name="eventType">The event to listen for.</param>
        /// <param name="handler">The method/function to call when the event is raised.</param>
        public static void AddObserver(Message eventType, Action<T,MessageArgs<U>> handler)
        {
            if (!_messageTable.TryGetValue(eventType, out List<Action<T, MessageArgs<U>>> handlers))
            {
                handlers = new List<Action<T, MessageArgs<U>>>();
                _messageTable.Add(eventType, handlers);
            }

            if (!handlers.Contains(handler))
            {
                _messageTable[eventType].Add(handler);
            }
        }

        /// <summary>
        /// Removes an event handler that was previously listening for a specific message that had 1 unit of data attached.
        /// </summary>
        /// <param name="eventType">The event to listen for.</param>
        /// <param name="handler">The method/function to call when the event is raised.</param>
        public static void RemoveObserver(Message eventType, Action<T, MessageArgs<U>> handler)
        {
            if (!_messageTable.TryGetValue(eventType, out List<Action<T, MessageArgs<U>>> handlers)) return;

            if (handlers.Contains(handler))
            {
                _messageTable[eventType].Remove(handler);
            }
        }

        /// <summary>
        /// Raises an event that calls all the listeners assigned to a specific message that includes 1 unit of data.
        /// Gracefully handles situations where there are no observers.
        /// </summary>
        /// <param name="eventType">The event to raise.</param>
        /// <param name="firstParameter">The first data to send with the message</param>
        /// <param name="secondParameter">A second set of data to include with the message</param>
        public static void SendEvent(Message eventType, T firstParameter, MessageArgs<U> secondParameter)
        {
            if (!_messageTable.TryGetValue(eventType, out List<Action<T, MessageArgs<U>>> handlers)) return;

            foreach (Action<T, MessageArgs<U>> handler in handlers)
            {
                handler(firstParameter, secondParameter);
            }
        }
        
        private static Dictionary<Message, List<Action<T, MessageArgs<U>>>> _messageTable = new Dictionary<Message, List<Action<T, MessageArgs<U>>>>();
    }
}