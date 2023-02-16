using System.Collections.Generic;

namespace Dark.Puriline
{
    public class Timeline
    {
        readonly Dictionary<string, Event> events = new();

        public void AddEvent(string key, Event @event) => events.Add(key, @event);
        public void RemoveEvent(string key) => events.Remove(key);

        /// <summary>
        /// Trying to get event from qhole timeline.
        /// </summary>
        /// <param name="key">Unique identifier of event.</param>
        /// <returns>An <see cref="Event"/> or null</returns>
        public Event GetEvent(string key)
        {
            events.TryGetValue(key, out Event @event);
            return @event;
        }
    }
}
