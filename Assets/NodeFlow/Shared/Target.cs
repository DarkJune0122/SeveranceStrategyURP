using System.Collections.Generic;
using UnityEngine;

namespace Dark.NodeFlow
{
    public class Target : MonoBehaviour
    {
        public static readonly List<Target> targets = new();

        private void Awake() => targets.Add(this);
        private void OnDestroy() => targets.Remove(this);
    }
}