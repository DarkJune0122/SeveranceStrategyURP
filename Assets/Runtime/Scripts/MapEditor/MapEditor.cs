using UnityEngine;

namespace SeveranceStrategy.MapEditor
{
    public sealed class MapEditor : MonoBehaviour
    {
        public static MapEditor Instance => m_instance;





        private static MapEditor m_instance;
        private MapEditor() => m_instance = this;
        public void Generate()
        {
            throw new System.NotImplementedException("Map generation not available yet.");
        }
    }
}