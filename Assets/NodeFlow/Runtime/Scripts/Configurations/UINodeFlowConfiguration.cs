using Dark.NodeFlow.Tools;
using Dark.NodeFlow.UI;
using System;
using UnityEngine;

namespace Dark.NodeFlow.Configuration
{
    [CreateAssetMenu]
    public sealed class UINodeFlowConfiguration : ScriptableObject
    {
        private static UINodeFlowConfiguration Instance;

        public static float TurningZone => Instance.m_turningZone;
        public static float ControlPointOffset => Instance.m_controlPointOffset;
        public static float Thickness => Instance.m_thickness;
        public static AnimationCurve ThicknessCurve => new(new Keyframe[] { new(0, Thickness) });
        public static float CurveStep => Instance.m_curveStep;
        public static BeizerCurve2D.Curvings Curving => Instance.m_curving;


        [Tooltip("How fast curve will turn to required direction")]
        [SerializeField, Range(0.01f, 1)] private float m_turningZone = 1;
        [Tooltip("Max curve step in evaluating Beizer curves")]
        [SerializeField, Range(0.01f, 1)] private float m_curveStep = 0.05f;
        [Tooltip("Control point offset from it's source (from start/end point)")]
        [SerializeField] private float m_controlPointOffset = 2.5f;
        [Tooltip("Curve thickness in units/pixels (dependent on space)")]
        [SerializeField] private float m_thickness = 0.5f;

        [Tooltip("Curving type")]
        [SerializeField] private BeizerCurve2D.Curvings m_curving = BeizerCurve2D.Curvings.Cubic;


        // Every configuration file should have initializer.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadConfig() => Instance = NodeLibrary.LoadConfiguration<UINodeFlowConfiguration>();

#if UNITY_EDITOR
        private void OnValidate()
        {
            UINodeWindow.allWindows.ForEach((w) => w.Rebuild());
        }
#endif
    }
}
