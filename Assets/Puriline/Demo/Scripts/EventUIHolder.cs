using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dark.Puriline.Demo
{
    /// <summary>
    /// Holds all <see cref="EventUI"/> objects and structurizes them by it's own.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class EventUIHolder : MonoBehaviour
    {
        readonly List<EventUI> uiEvents = new();

        public void AddEventUI(EventUI @event)
        {
            AddEventLocal(@event);
            Reorder();
        }
        public void AddEventUI(EventUI[] events)
        {
            for (int i = 0; i < events.Length; i++)
                AddEventLocal(events[i]);
            Reorder();
        }

        public void RemoveEventUI(EventUI @event)
        {
            RemoveEventLocal(@event);
            Reorder();
        }
        public void RemoveEventUI(EventUI[] events)
        {
            for (int i = 0; i < events.Length; i++)
                RemoveEventLocal(events[i]);
            Reorder();
        }


        private void AddEventLocal(EventUI @event)
        {
            uiEvents.Add(@event);
            @event.Event.onFinalize += Event_OnDispose;
        }
        private void RemoveEventLocal(EventUI @event)
        {
            int index = uiEvents.FindIndex(x => x == @event);
            uiEvents[index].Event.onFinalize -= Event_OnDispose;
            Destroy(uiEvents[index].gameObject);
            uiEvents.RemoveAt(index);
        }
        private void Event_OnDispose(Event @event, bool isSuccessful)
        {
            int index = uiEvents.FindIndex(x => x.Event == @event);
            Destroy(uiEvents[index].gameObject);
            uiEvents.RemoveAt(index);
        }


        private void Reorder()
        {
            uiEvents.Sort(new EventUI.PriorityComparer());
            foreach (EventUI @event in uiEvents)
            {
                @event.transform.SetAsFirstSibling();
            }
        }
    }
}
