using System;
using System.Collections.Generic;
using UnityEngine;

namespace SeveranceStrategy.Buildings
{
    public class DynamicInstance : MonoBehaviour
    {
        public static readonly Dictionary<string, DynamicInstance> instances = new();
        [SerializeField, NonSerialized] protected float m_maxHealth;
    }
}