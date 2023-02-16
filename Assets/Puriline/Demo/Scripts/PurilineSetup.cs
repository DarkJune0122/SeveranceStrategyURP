using UnityEngine;

namespace Dark.Puriline.Demo
{
    public sealed class PurilineSetup : MonoBehaviour
    {
        public static PurilineSetup Instance { get; private set; }

        [SerializeField] EventUI m_eventPrefab;
        [SerializeField] EventUIHolder m_eventHolder;
        [SerializeField] Event[] m_eventList;


        private void Awake()
        {
            Instance = this;
            for (int eventIndex = 0; eventIndex < m_eventList.Length; eventIndex++)
                RegisterEvent(m_eventList[eventIndex]);
        }

        public void RegisterEvent(Event @event)
        {
            EventUI eventUI = Instantiate(m_eventPrefab, m_eventHolder.transform);
            eventUI.Initialize(@event);
            eventUI.EventButton.onClick.AddListener(() => eventUI.Event.Finalize(true));
            m_eventHolder.AddEventUI(eventUI);
        }
    }
}
