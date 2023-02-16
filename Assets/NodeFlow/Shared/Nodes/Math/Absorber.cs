using System;

namespace Dark.NodeFlow.Demo
{
    public class Absorber : Node
    {
        // Inner
        public FlowNative<float> in_value;

        // Outher
        public readonly FlowNative<float> out_time = new();


        private void Update()
        {
            out_time.Value = Math.Abs(in_value.Value);
        }
    }
}