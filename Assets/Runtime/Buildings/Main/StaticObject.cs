using System.Collections.Generic;
using UnityEngine;

namespace SeveranceStrategy.Buildings
{
    public class StaticObject
    {
        public static readonly Dictionary<string, StaticObject> classPool = new();
        public virtual string ClassName => nameof(StaticObject);
        public Vector2Int size;
        
        public StaticObject() => classPool.Add(ClassName, this);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">Local position of new GameObject.</param>
        /// <param name="parent">Parent of GameObject. Provide Null value to make it to be placed directly on grid.</param>
        public virtual T Create<T>(Vector2 position, Transform parent) where T : StaticInstance
        {
            return new GameObject("temp").AddComponent<T>();
        }
    }
}
