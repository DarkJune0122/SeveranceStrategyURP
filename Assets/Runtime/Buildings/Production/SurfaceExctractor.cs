using SeveranceStrategy.Buildings.Sources;
using UnityEngine;

namespace SeveranceStrategy.Buildings.Production
{
    public class SurfaceExctractor : DynamicInstance
    {
        public SurfaceOre Ore
        {
            get => m_ore;
            set
            {
                m_ore = value;
                enabled = value != null;
            }
        }

        [SerializeField] protected string[] m_ores;
        [SerializeField] protected float m_speed;


        protected SurfaceOre m_ore;
        protected float m_time;
        protected virtual void Update()
        {
            
        }
    }
}