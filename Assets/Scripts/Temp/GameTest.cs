using SeveranceStrategy.Protoinfo;
using SeveranceStrategy.Prototypes.Sources;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SeveranceStrategy
{
    public sealed class GameTest : MonoBehaviour
    {
        [SerializeField] private BlockInfo m_prototype;
        [SerializeField] private Transform m_anchor;

        private void Awake()
        {
            OreInstance ore = m_prototype.CreatePrototype<OreInstance>(m_anchor);
            ore.transform.localPosition = ore.transform.localPosition.Set_Y(ore.transform.localPosition.y + ore.transform.localScale.y * 0.5f);
            //BlockInfo.GetInfo<OreInfo>(m_prototype.name).CreatePrototype<OreInstance>(m_anchor);
        }


#if UNITY_EDITOR
        [CustomEditor(typeof(GameTest))]
        private class Editor_GameTest : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                GameTest obj = (GameTest)target;
            }
        }
#endif
    }
}