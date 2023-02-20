using System;
using UnityEngine;

namespace SeveranceStrategy.Inputs
{
    [Serializable]
    [Obsolete("Most likely, will not be supported further.")]
    public sealed class InputEntry
    {
        /// <summary>
        /// Name of input entry.
        /// Can be used to visualize entry on UI.
        /// </summary>
        public readonly string name;

        /// <summary>
        /// What key entry receaves.
        /// </summary>
        public readonly KeyCode key;

        /// <summary>
        /// Should return TRUE if button press event has to be eaten.
        /// </summary>
        public readonly Func<bool> func;

        public InputEntry(KeyCode key, Func<bool> func, string name = null)
        {
            this.key = key;
            this.func = func;
            this.name = name;
        }
    }
}