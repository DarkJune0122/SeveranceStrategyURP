using UnityEngine;
using UnityEngine.Timeline;

namespace SeveranceStrategy.Separation
{
    /// <summary>
    /// Just for testing various things.
    /// </summary>
    public class Separate : MonoBehaviour
    {
        public static Separate Instance { get; private set; }

        public float spacing = 0.25f;
        public int targetIndex = 1;
        public int blockCount = 5;




#if UNITY_EDITOR
        public sealed class Editor_Separate : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (!Application.isPlaying)
                    return;

                GUILayout.Space(5f);
                if (GUILayout.Button("Reorder"))
                {

                }
            }
        }
#endif
    }
}