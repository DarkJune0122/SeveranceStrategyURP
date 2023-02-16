using UnityEngine;
using UnityEngine.EventSystems;

namespace Dark.ModularUnits.Demo
{
    public abstract class RaycastingBehaviour : MonoBehaviour
    {
        public BoxCollider2D RaycastHandler => m_raycastHandler;

        [Header(nameof(RaycastingBehaviour))]
        [SerializeField] protected BoxCollider2D m_raycastHandler;

        public abstract void OnPointerDown(PointerEventData eventData);
    }
}