using System.Collections.Generic;
using UnityEngine;

namespace SeveranceStrategy.FXs.GameEffects
{
    /// <summary>
    /// Base Interface for all in-game effects.
    /// </summary>
    public abstract class GameEffect
    {
        public static readonly Dictionary<string, GameEffect> All = new();


        public GameEffect(string name)
        {

        }

        /// <summary>
        /// Starts an effect at given point of game world.
        /// </summary>
        /// <param name="position">Real world position.</param>
        /// <param name="args">Arguments for bame effect.</param>
        public abstract void StartAt(Vector2 position, params object[] args);
    }
}
