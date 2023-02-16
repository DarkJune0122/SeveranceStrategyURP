using UnityEngine;
using UnityEngine.UI;

namespace Dark.NodeFlow.UI
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class UISocket : MonoBehaviour
    {
        public bool Interactible
        {
            get => m_raycastHandler.raycastTarget;
            set => m_raycastHandler.raycastTarget = value;
        }
        public bool IsOuter => m_isOuter;
        public UIFlow Flow => m_flow;

        [SerializeField] private Graphic m_raycastHandler;
        [Header("Dynamics")]
        [SerializeField] private UIFlow m_flow;
        [SerializeField] private bool m_isOuter;

        public void Setup(UIFlow flow, bool isOuter, bool interactible)
        {
            m_flow = flow;
            m_isOuter = isOuter;
            Interactible = interactible;

            RectTransform rectTransform = (RectTransform)transform;
            rectTransform.anchorMin = new Vector2(isOuter ? 1 : 0, 0);
            rectTransform.anchorMax = new Vector2(isOuter ? 1 : 0, 0);
            rectTransform.pivot = new Vector2(isOuter ? 1 : 0, 1);
        }
        /// <inheritdoc cref="Setup(UIFlow, bool, bool)"/>
        public void Setup(UIFlow flow, bool isOuter) => Setup(flow, isOuter, true);
    }
}
