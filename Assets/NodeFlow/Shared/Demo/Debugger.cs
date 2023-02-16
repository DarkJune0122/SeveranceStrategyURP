using UnityEngine;

namespace Dark.NodeFlow.Demo
{
    public class Debugger : MonoBehaviour
    {
        // Inner
        public FlowNative<float> in_debug;

        // Outher


        // Update is called once per frame
        void Update() => Debug.Log(in_debug.Value);
    }
}