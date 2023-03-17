using SeveranceStrategy.Prototypes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SeveranceStrategy.Protoinfo
{
    [CreateAssetMenu(menuName = "ProtoInfo/" + nameof(BlockInfo))]
    public class BlockInfo : ScriptableObject
    {
        private static readonly Dictionary<string, BlockInfo> all = new();

        [SerializeField] protected Texture2D m_icon;
        [Tooltip("Path to object prototype")]
        [SerializeField] protected string m_prototype = Core.Environment.PrototypePrefix;
        [Tooltip("Whether the object prototype should NOT be unloaded from memory.\nThis will improve perfomance and reduce freezes")]
        [Obsolete(Core.Environment.AwaitsForSupport)]
        [SerializeField] protected bool m_preventUnloading;

        private void Awake() => all.Add(name, this);
        public static T GetInfo<T>(string prototypeName) where T : BlockInfo
        {
            if (!all.TryGetValue(prototypeName, out BlockInfo info)) throw new Exception("Prototype not found! " + prototypeName);
            if (info is not T item) throw new Exception($"Given prototype not match the given type!\nPrototype: {prototypeName}  Type: {typeof(T)}");
            return item;
        }

        public T CreatePrototype<T>(Transform parent) where T : BlockIntance => CreatePrototype<T>(Vector3.zero, Quaternion.identity, parent);
        public T CreatePrototype<T>(Vector3 position, Quaternion rotation, Transform parent) where T : BlockIntance
        {
            if (m_prototype.StartsWith(Core.Environment.PrototypePrefix))
            {
                T resource = Resources.Load<T>(m_prototype);
                T obj = Instantiate(resource, parent);
                obj.transform.localPosition = position;
                obj.transform.localRotation = rotation;
                obj.Setup(this);
                return obj;
            }
            else throw Core.Environment.AddressableAssetsSupport;
        }
    }
}