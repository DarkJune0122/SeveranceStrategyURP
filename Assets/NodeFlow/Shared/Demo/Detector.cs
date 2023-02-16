using UnityEngine;

namespace Dark.NodeFlow.Demo
{
    public class Detector : Node
    {
        // Inner
        // Outher
        public readonly FlowClass<Target> out_target = new();

        [SerializeField] float maxSqrDistance = 4f * 4f;

        private void Update()
        {
            if (out_target.Value == null)
            {
                foreach(Target target in Target.targets)
                {
                    if ((target.transform.position - transform.position).sqrMagnitude > maxSqrDistance)
                        continue;

                    out_target.Value = target;
                    break;
                }
            }
            else
            {
                if ((out_target.Value.transform.position - transform.position).sqrMagnitude > maxSqrDistance)
                    out_target.Value = null;
            }
        }
    }
}