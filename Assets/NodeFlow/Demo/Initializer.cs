using Dark.NodeFlow;
using Dark.NodeFlow.Demo;
using System.Text;
using UnityEngine;

public sealed class Initializer : MonoBehaviour
{
    public MonoBehaviour[] m_nodes;
    public Timer m_timer;
    public Siner[] m_siners;
    public Debugger m_debugger;
    public float m_totalSpaceX = 6;
    [Space(5f)]
    public int inOffset;
    public int outOffset;


    public void Reorder()
    {
        float maxStep = m_totalSpaceX / m_nodes.Length;
        float offsetX = m_totalSpaceX * -0.5f;
        for (int i = 0; i < m_nodes.Length; i++)
        {
            m_nodes[i].transform.position = new Vector3(offsetX + maxStep * i, 0, 0);
        }
    }

    public void InitalizeFlows()
    {
        m_siners[0].in_time = m_timer.out_time;
        m_debugger.in_debug = m_siners[^1].out_sine;

        for(int i = 1; i < m_siners.Length; i++)
        {
            m_siners[i].in_time = m_siners[i - 1].out_sine;
        }
    }

    public void VisualizeFlows()
    {
        MonoBehaviour[] nodes = new MonoBehaviour[m_nodes.Length - inOffset - outOffset];
        System.Array.Copy(m_nodes, inOffset, nodes, 0, nodes.Length);

        NodeLib.Instance.NodeWindow.VisualizeNodes(nodes);
    }



#if UNITY_EDITOR
    private void OnValidate()
    {
        m_nodes = new MonoBehaviour[m_siners.Length + 2];
        for (int i = 1, len = m_nodes.Length - 1; i < len; i++)
        {
            m_nodes[i] = m_siners[i - 1];
        }
        m_nodes[0] = m_timer;
        m_nodes[^1] = m_debugger;
    }

    [UnityEditor.CustomEditor(typeof(Initializer))]
    private class Editor_Initializer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Initializer initializer = (Initializer)target;
            if (GUILayout.Button(nameof(Initializer.Reorder)))
            {
                initializer.Reorder();
            }

            if (initializer.m_nodes?.Length == 0)
                return;

            int delta = initializer.m_nodes.Length - initializer.inOffset - initializer.outOffset;
            if (delta <= 0)
            {
                GUILayout.Label($"Offsets are too hight! ({-delta})");
                return;
            }

            char fillSymbol = '□';
            char spacingSymbol = '=';
            StringBuilder builder = new();
            for (int i = initializer.inOffset; i > 0; i--)
                builder.Append(spacingSymbol);

            for (int i = delta; i > 0; i--)
                builder.Append(fillSymbol);

            for (int i = initializer.outOffset; i > 0; i--)
                builder.Append(spacingSymbol);

            GUILayout.Label(builder.ToString());

            if (NodeLib.Instance == null)
                return;

            if (GUILayout.Button("Visualize Nodes"))
            {
                initializer.InitalizeFlows();
                initializer.VisualizeFlows();
            }
        }
    }
#endif
}
