using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dark.Puriline
{
    [Serializable]
    public class Event
    {
        /// <summary>
        /// An unique name of event.
        /// </summary>
        public string Name => m_name;

        /// <summary>
        /// An unique name of event.
        /// </summary>
        public short Priority => m_priority;


        /// <summary>
        /// Calls whatever event is finished or disposed. Hands over True if event was finished successfully (See also <seealso cref="Finalize(bool)"/>)
        /// </summary>
        public event Action<Event, bool> onFinalize;

        [SerializeField] private string m_name;
        [SerializeField] private short m_priority;


        public Event(string name) => m_name = name;

        /// <summary>
        /// Finishes this event.
        /// </summary>
        /// <param name="isSuccessful">Whether this event is ended successfully.</param>
        public virtual void Finalize(bool isSuccessful) => onFinalize?.Invoke(this, isSuccessful);


        public class PriorityComparer : IComparer<Event>
        {
            public int Compare(Event x, Event y) => x.Priority.CompareTo(y.Priority);
        }
    }
}
