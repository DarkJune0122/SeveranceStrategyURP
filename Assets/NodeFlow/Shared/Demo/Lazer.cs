using UnityEngine;

namespace Dark.NodeFlow.Demo
{
    public class Lazer : Node
    {
        // Inner
        public FlowClass<Target> in_target;
        // Outher

        [SerializeField] LineRenderer line;

        void Update()
        {
            if (in_target.Value == null)
            {
                line.positionCount = 0;
            }
            else
            {
                line.positionCount = 2;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, in_target.Value.transform.position);
            }
        }
    }
}