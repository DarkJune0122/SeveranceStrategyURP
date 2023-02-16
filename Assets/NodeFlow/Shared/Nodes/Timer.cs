using UnityEngine;

namespace Dark.NodeFlow.Demo
{
    public class Timer : Node
    {
        // Inner

        // Outher
        public readonly FlowNative<float> out_time = new();


        private void Update()
        {
            out_time.Value += Time.deltaTime;
        }
    }
}
