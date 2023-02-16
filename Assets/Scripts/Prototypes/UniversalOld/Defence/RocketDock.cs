using SeveranceStrategy.Modularity;
using UnityEngine;

namespace SeveranceStrategy.Special
{
    public class RocketDock : DestroyableObject
    {
        /// <summary>
        /// Current acceptable rocket rank.
        /// </summary>
        public int RocketRank => m_rocketRank;

        [SerializeField] protected int m_rocketRank;
        [SerializeField] protected TypeSocket[] m_socket;
    }
}
