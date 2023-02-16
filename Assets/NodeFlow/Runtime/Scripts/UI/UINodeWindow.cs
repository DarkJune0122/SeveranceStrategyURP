using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dark.NodeFlow.UI
{
    public sealed class UINodeWindow : MonoBehaviour
    {
        private struct FlowInfo
        {
            /// <summary>
            /// Actual flow reference.
            /// </summary>
            public Flow flow;
            /// <summary>
            /// Whether is flow reference that gives value.
            /// </summary>
            public bool isOuter;
        }


        [SerializeField] UINode[] m_nodes;
        [Tooltip("Spacing in screen pixels.")]
        [SerializeField] Vector2 m_nodeSpacing = new(x: 10f, y: 10f);
        [Header("Presets")]
        [SerializeField] UINode m_nodePreset;
        [SerializeField] UIFlow m_flowPreset;
        [SerializeField] UISocket m_socketPreset;

        /// <summary>
        /// Visualizes all given objects in graph view.
        /// </summary>
        /// <param name="objs">all your objects</param>
        public void VisualizeNodes(Object[] objs)
        {
            ResetUI();
            m_nodes = new UINode[objs.Length];
            List<UIFlow> uiFlows = new();
            for (int i = 0; i < m_nodes.Length; i++)
            {
                FlowInfo[] flowInfo = GetFlows(objs[i]);
                if (flowInfo.Length == 0)
                    continue;

                m_nodes[i] = Instantiate(m_nodePreset, transform);

                // Creates sockets (for current node) and flows (for all nodes)
                UISocket[] sockets = new UISocket[flowInfo.Length];
                for (int i2 = 0; i2 < sockets.Length; i2++)
                {
                    // Creates UI object instances.
                    sockets[i2] = Instantiate(m_socketPreset, m_nodes[i].m_socketAnchor);

                    if (FlowExist(out UIFlow flow))
                    {
                        // Adds new socket reference to flow instance.
                        flow.EndCount++;
                        flow.SetSocket(flow.EndCount - 1, sockets[i2]);
                    }
                    else
                    {
                        // Creates new flow.
                        flow = Instantiate(m_flowPreset, sockets[i2].transform);
                        uiFlows.Add(flow);

                        // Initializes flow values.
                        flow.Setup(flowInfo[i2].flow, sockets[i2]);
                    }

                    // Initializes socket.
                    sockets[i2].Setup(flow, flowInfo[i2].isOuter);


                    // Checks are curently iterated flow (flows[i2].flow) is exitst.
                    bool FlowExist(out UIFlow flow)
                    {
                        // Finds already existing flow.
                        Flow targetFlow = flowInfo[i2].flow;
                        flow = uiFlows.Find((flow) => flow.Flow == targetFlow);
                        return flow != null;
                    }
                }


                // Adds sockets to it's parent - UINode instance.
                int count = GetOutSocketsCount();
                UISocket[] outSockets = new UISocket[count];
                UISocket[] inSockets = new UISocket[sockets.Length - count];
                for (int i3 = 0, inCount = 0, outCount = 0; i3 < sockets.Length; i3++)
                {
                    if (sockets[i3].IsOuter)
                    {
                        outSockets[outCount] = sockets[i3];
                        outCount++;
                    }
                    else
                    {

                        inSockets[inCount] = sockets[i3];
                        inCount++;
                    }
                }

                m_nodes[i].m_inSockets = inSockets;
                m_nodes[i].m_outSockets = outSockets;

                // Return: amount of outer sockets in this node.
                int GetOutSocketsCount()
                {
                    int count = 0;
                    for (int i = sockets.Length - 1; i > -1; i--)
                    {
                        if (sockets[i].IsOuter)
                            count++;
                    }
                    return count;
                }


                // Layout Rebuild.
                m_nodes[i].RebuildLayout();

                // Placing node in required position on UI (Temporaly direct layout implementation).
                RectTransform nodeRect = (RectTransform)m_nodes[i].transform;
                nodeRect.anchorMin = new Vector2(0, 0.5f);
                nodeRect.anchorMax = new Vector2(0, 0.5f);
                nodeRect.pivot = new Vector2(0, 0.5f);
                nodeRect.anchoredPosition = new Vector2(nodeRect.sizeDelta.x * i + m_nodeSpacing.x * (i + 1), 0);
            }

            // Rest UI updates.
            Rebuild();


            // Uses reflections to get all flows from given object.
            FlowInfo[] GetFlows(Object obj)
            {
                List<FlowInfo> flows = new();
                System.Reflection.FieldInfo[] fields = obj.GetType().GetFields();
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i].FieldType.BaseType == null)
                        continue;

                    if (fields[i].FieldType.BaseType == typeof(Flow))
                    {
                        flows.Add(new FlowInfo()
                        {
                            flow = (Flow)fields[i].GetValue(obj),
                            isOuter = fields[i].IsInitOnly,
                        });
                    }
                }
                return flows.ToArray();
            }
        }

        /// <summary>
        /// Destroys all UI objects and clears references.
        /// </summary>
        public void ResetUI()
        {
            for (int i = 0; i < m_nodes.Length; i++)
                Destroy(m_nodes[i].gameObject);

            m_nodes = null;
        }



        /// <summary>
        /// Rebuilds flow graphics (Curves) immediate.
        /// </summary>
        public void RebuildImmediate()
        {
            for (int i = 0; i < m_nodes.Length; i++)
            {
                m_nodes[i].RebuildFlows();
            }
        }

        bool isDirty;
        public void Rebuild()
        {
            if (isDirty)
                return;

            isDirty = true;
            StartCoroutine(LateRebuild());
            IEnumerator LateRebuild()
            {
                yield return null;
                RebuildImmediate();
                isDirty = false;
            }
        }



#if UNITY_EDITOR
        public static readonly List<UINodeWindow> allWindows = new();
        private void Awake() => allWindows.Add(this);
        private void OnDestroy() => allWindows.Remove(this);
        

        [UnityEditor.CustomEditor(typeof(UINodeWindow))]
        public sealed class Editor_UINodeWindow : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (GUILayout.Button("Rebuild UI"))
                {
                    ((UINodeWindow)target).Rebuild();
                }
            }
        }
#endif
    }
}