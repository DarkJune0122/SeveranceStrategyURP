using UnityEngine.UI;

namespace Dark.NetShared
{
    // Original available by link:
    // https://answers.unity.com/questions/1091618/ui-panel-without-image-component-as-raycast-target.html
    // Author: https://answers.unity.com/users/153967/slippdouglas.html?inRegister=true

    /// <summary>
    /// A concrete subclass of the Unity UI `Graphic` class that just skips drawing.
    /// Useful for providing a raycast target without actually drawing anything.
    /// </summary>
    public class NonDrawingGraphic : Graphic
    {
        public override void SetMaterialDirty() { return; }
        public override void SetVerticesDirty() { return; }

        /// <summary>
        /// Probably not necessary since the chain of calls `Rebuild()`->`UpdateGeometry()`->`DoMeshGeneration()`->`OnPopulateMesh()` won't happen.
        /// So here really just as a fail-safe.
        /// </summary>
        /// <param name="vh"></param>
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            return;
        }
    }
}