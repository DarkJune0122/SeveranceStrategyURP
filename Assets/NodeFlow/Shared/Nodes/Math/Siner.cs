using UnityEngine;

namespace Dark.NodeFlow.Demo
{
    public class Siner : Node
    {
        // Inner
        public FlowNative<float> in_time;

        // Outher
        public readonly FlowNative<float> out_sine = new();
        public readonly FlowNative<float> out_garbage = new();


        // Update is called once per frame
        void Update()
        {
            out_sine.Value = Mathf.Sin(in_time.Value);
        }
    }
}