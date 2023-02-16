using System;

namespace Dark.Puriline
{

    [Serializable]
    public sealed class Event<T> : Event
    {
        /// <summary>
        /// Current event state.
        /// </summary>
        public T State
        {
            get => m_state;
            set
            {
                m_state = value;
                onEventStateChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Calls whatever <see cref="State"/> of this event was changed.
        /// </summary>
        public event Action<T> onEventStateChanged;


        /// <inheritdoc cref="State"/>
        private T m_state = default;
        public Event(string name) : base(name) { }
    }
}
