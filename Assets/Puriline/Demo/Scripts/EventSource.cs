using UnityEngine;

namespace Dark.Puriline.Demo
{
    public class EventSource : MonoBehaviour
    {
        [SerializeField] Event ownEvent;

        private void Start() => PurilineSetup.Instance.RegisterEvent(ownEvent);

        private void OnEnable() => ownEvent.onFinalize += OwnEvent_onFinalize;
        private void OnDisable() => ownEvent.onFinalize -= OwnEvent_onFinalize;


        private void OwnEvent_onFinalize(Event @event, bool isSuccessful)
        {
            transform.localScale *= 2;
        }
    }
}
