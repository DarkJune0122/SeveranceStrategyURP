using UnityEditor;

namespace Dark.NodeFlow.Editor
{
    public sealed class NodeConfigurationWindow : OptiLib.Editor.EditorWindow
    {
        protected override string EditorName => "Node Configurations";

        [MenuItem("NodeScripting/Configurations")]
        private static void Init() => OpenWindow<NodeConfigurationWindow>();

        private void OnValidate()
        {
            EditorUtility.SetDirty(this);
        }
    }
}
