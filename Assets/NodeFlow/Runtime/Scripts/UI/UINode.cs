using UnityEngine;

namespace Dark.NodeFlow.UI
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class UINode : MonoBehaviour
    {
        public RectTransform m_socketAnchor;
        public UISocket[] m_inSockets;
        public UISocket[] m_outSockets;

        Vector2 initialSizeDelta;
        // Vector3 position;
        private void Awake() => initialSizeDelta = ((RectTransform)transform).sizeDelta;

        /*void Update()
        {
            if (position == transform.position)
                return;

            Vector3 pos = transform.position;
            pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
            position = transform.position = pos;
            UINodeWindow.allWindows.ForEach((w) => w.RebuildUI());
        }*/

        /// <summary>
        /// Recalculates positions of <see cref="UISocket"/>s.
        /// </summary>
        public void RebuildLayout()
        {
            RectTransform rectTransform = (RectTransform)transform;
            rectTransform.sizeDelta = new Vector2(x: rectTransform.sizeDelta.x,
                                                  y: initialSizeDelta.y + Mathf.Max(PerformLayout(m_inSockets), PerformLayout(m_outSockets)));

            // Applies layout rules to given socket array.
            // Returns: Total Y offset for all gives sockets.
            static float PerformLayout(UISocket[] sockets)
            {
                float yOffset = 0;
                for (int i = 0; i < sockets.Length; i++)
                {
                    RectTransform socketRect = (RectTransform)sockets[i].transform;
                    socketRect.anchoredPosition = new(0f, yOffset); // X value controlled by min/max values of socket.
                    yOffset -= socketRect.sizeDelta.y;
                }
                return -yOffset;
            }
        }

        public void RebuildFlows()
        {
            for (int i = 0; i < m_outSockets.Length; i++)
            {
#if UNITY_EDITOR
                m_outSockets[i].Flow.DebugRebuild();
#else
                m_outSockets[i].Flow.Rebuild();
#endif
            }
        }
    }
}