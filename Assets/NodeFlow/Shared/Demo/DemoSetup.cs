using UnityEngine;

namespace Dark.NodeFlow.Demo
{
    public class DemoSetup : MonoBehaviour
    {
        [SerializeField] Detector detector;
        [SerializeField] Lazer lazer;

        private void Awake()
        {
            lazer.in_target = detector.out_target;
        }
    }
}