using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dark.Puriline.Demo
{
    public class EventUI : MonoBehaviour
    {
        public Event Event => alignedEvent;
        public Button EventButton => m_eventButton;

        [SerializeField] private Text m_eventTitle;
        [SerializeField] private Button m_eventButton;



        private Event alignedEvent;
        public void Initialize(Event @event)
        {
            alignedEvent = @event;
            UpdateUIValues();
        }

        protected virtual void UpdateUIValues()
        {
            m_eventTitle.text = alignedEvent.Name;
        }


        // Classes and etc.
        public class PriorityComparer : IComparer<EventUI>
        {
            public int Compare(EventUI x, EventUI y) => x.alignedEvent.Priority.CompareTo(y.alignedEvent.Priority);
        }
    }
}
