using UnityEditor;

namespace Dark.OptiLib.Editor
{
    public abstract class EditorWindow : UnityEditor.EditorWindow
    {
        protected abstract string EditorName { get; }

        protected static T OpenWindow<T>() where T : EditorWindow
        {
            T window = GetWindow<T>();
            window.titleContent = EditorGUIUtility.TrTextContent(window.EditorName);
            return window;
        }
    }
}