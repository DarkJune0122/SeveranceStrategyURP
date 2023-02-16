using System;

namespace Dark.NodeFlow
{
    /// <summary>
    /// Flow impementation for <![CDATA[ classes ]]>.
    /// </summary>
    [Serializable]
    public sealed class FlowClass<T> : Flow where T : class
    {
        public T Value;

        /// <inheritdoc cref="Flow.IsInvalid"/>
        /// <remarks>
        /// Is TRUE if <see cref="Value"/> is NULL.
        /// </remarks>
        public override bool IsInvalid => Value == null;
    }

    /// <summary>
    /// Flow impementation for <![CDATA[ immutable objects ]]>.
    /// </summary>
    [Serializable]
    public sealed class FlowNative<T> : Flow where T : unmanaged
    {
        public T Value;

        /// <inheritdoc cref="Flow.IsInvalid"/>
        /// <remarks>
        /// In <see cref="FlowNative{T}"/> it allways false.
        /// </remarks>
        public override bool IsInvalid => false;
    }

    public abstract class Flow
    {
        /// <summary>
        /// Are flow has invalid or unusable value.
        /// </summary>
        public abstract bool IsInvalid { get; }
    }
}
