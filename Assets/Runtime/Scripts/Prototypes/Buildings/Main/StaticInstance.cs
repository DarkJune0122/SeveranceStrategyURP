using System.Collections.Generic;
using UnityEngine;

namespace SeveranceStrategy.Buildings
{
    public class StaticInstance : MonoBehaviour
    {
        public static readonly Dictionary<string, StaticInstance> instances = new();
    }
}