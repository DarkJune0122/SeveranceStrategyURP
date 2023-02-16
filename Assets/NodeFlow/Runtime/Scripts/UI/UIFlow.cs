using Dark.NodeFlow.Configuration;
using Dark.NodeFlow.Tools;
using System;
using System.Collections;
using UnityEngine;

namespace Dark.NodeFlow.UI
{
    public sealed class UIFlow : MonoBehaviour
    {
        public Flow Flow => m_flow;
        public UISocket Source
        {
            get => m_source;
            set
            {
                m_source = value;
                Rebuild();
            }
        }
        /// <summary>
        /// Array of flow ends.
        /// </summary>
        public UISocket[] End => m_end;
        public int EndCount
        {
            get => m_end.Length;
            set => Array.Resize(ref m_end, value);
        }

        [SerializeField] private LineRenderer m_lineRenderer;

        /// <inheritdoc cref="Flow"/>
        private Flow m_flow;
        /// <inheritdoc cref="Source"/>
        private UISocket m_source;
        /// <inheritdoc cref="End"/>
        private UISocket[] m_end = new UISocket[0];


        /// <summary>
        /// UIFlow setup method for range of it's values.
        /// It also can be called after initialization without side effects.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="end"></param>
        public void Setup(Flow flow, UISocket source, params UISocket[] end)
        {
            m_flow = flow;
            m_source = source;
            m_end = end;
            Rebuild();
        }
        /// <inheritdoc cref="Setup(Flow, UISocket, UISocket[])"/>
        public void Setup(Flow flow, UISocket source) => Setup(flow, source, new UISocket[0]);
        /// <inheritdoc cref="Setup(Flow, UISocket, UISocket[])"/>
        public void Setup(Flow flow) => Setup(flow, null, new UISocket[0]);
        public void Awake() => m_lineRenderer.widthCurve = UINodeFlowConfiguration.ThicknessCurve;

        public void SetSocket(int index, UISocket socket)
        {
            m_end[index] = socket;
            Rebuild();
        }


        /// <summary>
        /// Calls when UI objects should check are all it's references valid.
        /// </summary>
        public void UIValidate()
        {
            if (m_flow == null)
            {
                m_source = null;
                m_end = new UISocket[0];
                return;
            }
        }

        /// <summary>
        /// Rebuilds flow graphics (Curves) immediate.
        /// </summary>
        public void RebuildImmediate()
        {
            if (m_source == null || m_end.Length == 0)
            {
                m_lineRenderer.positionCount = 0;
                return;
            }

            // Builds cubic curve between points.
            Vector2[] points = GetCurve(m_end[0]);

            // Applies all points to line renderer.
            m_lineRenderer.positionCount = points.Length;
            for (int i = points.Length - 1; i > -1; i--)
            {
                m_lineRenderer.SetPosition(i, new Vector3(points[i].x, points[i].y, 0));
            }

            Vector2[] GetCurve(UISocket end) => UINodeFlowConfiguration.Curving switch
            {
                BeizerCurve2D.Curvings.Cubic => BeizerCurve2D.CubicCurve(
                    source: m_source.transform.position,
                    control1: new Vector2(x: m_source.transform.position.x + UINodeFlowConfiguration.ControlPointOffset, m_source.transform.position.y),
                    end: end.transform.position,
                    control2: new Vector2(x: end.transform.position.x - UINodeFlowConfiguration.ControlPointOffset, end.transform.position.y),
                    step: UINodeFlowConfiguration.CurveStep),

                BeizerCurve2D.Curvings.Direct => BeizerCurve2D.DirectCurve(
                    source: m_source.transform.position,
                    control1: new Vector2(x: m_source.transform.position.x + UINodeFlowConfiguration.ControlPointOffset, m_source.transform.position.y),
                    end: end.transform.position,
                    control2: new Vector2(x: end.transform.position.x - UINodeFlowConfiguration.ControlPointOffset, end.transform.position.y),
                    step: UINodeFlowConfiguration.CurveStep,
                    turningZone: UINodeFlowConfiguration.TurningZone),

                BeizerCurve2D.Curvings.Linear => new Vector2[] { m_source.transform.position, end.transform.position },

                _ => throw new NotImplementedException(),
            };
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
        private void OnValidate()
        {
            if (m_source == null || m_end.Length == 0)
                return;

            Rebuild();
        }

        public void DebugRebuild()
        {
            Awake();
            OnValidate();
        }

        [UnityEditor.CustomEditor(typeof(UIFlow))]
        public sealed class Editor_UIFlow : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (!Application.isPlaying)
                    return;

                if (GUILayout.Button("Rebuild UI"))
                {
                    ((UIFlow)target).RebuildImmediate();
                }
            }
        }
#endif
    }
}
