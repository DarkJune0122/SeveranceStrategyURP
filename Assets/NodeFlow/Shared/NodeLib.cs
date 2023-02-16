using Dark.NodeFlow.UI;
using UnityEngine;

namespace Dark.NodeFlow
{
    /// <summary>
    /// Contains data, that can be used by nodes
    /// </summary>
    public class NodeLib : MonoBehaviour
    {
        public static NodeLib Instance { get; private set; }
        public UINodeWindow NodeWindow => m_nodeWindow;


        [SerializeField] private UINodeWindow m_nodeWindow;


        private void Awake() => Instance = this;
    }
}