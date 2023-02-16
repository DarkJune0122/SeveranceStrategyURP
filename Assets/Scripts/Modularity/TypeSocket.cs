using System;
using System.Collections.Generic;
using UnityEngine;

namespace SeveranceStrategy.Modularity
{
    public sealed class TypeSocket : Dark.Modularity.ModuleSocket
    {
        private readonly HashSet<Type> m_filters;
        private readonly bool m_whitelist;
        public TypeSocket(Vector2 localPosition) : base(localPosition) { }
        public TypeSocket(Vector2 localPosition, HashSet<Type> filters, bool whitelist) : base(localPosition)
        {
            m_filters = filters;
            m_whitelist = whitelist;
        }


        /// <summary>
        /// Checks whether is given filters allows to use given type.
        /// </summary>
        /// <param name="type">Your type to be filterred</param>
        /// <returns>True whether given <paramref name="type"/> is valid for usage with socket. False if filter doesn't allow performing actions.</returns>
        public bool Filter(Type type) => m_filters?.Contains(type) == m_whitelist;
    }
    public sealed class SizeSocket : Dark.Modularity.ModuleSocket
    {
        private readonly uint m_maxSize;
        public SizeSocket(Vector2 localPosition) : base(localPosition) { }
        public SizeSocket(Vector2 localPosition, uint minSize) : base(localPosition) => m_maxSize = minSize;


        /// <summary>
        /// Checks whether is given filters allows to use given type.
        /// </summary>
        public bool Filter(uint size) => m_maxSize <= size;
    }
}
