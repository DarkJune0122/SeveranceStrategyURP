using SeveranceStrategy.Buildings;
using UnityEngine;

namespace SeveranceStrategy.Environment
{
    [RequireComponent(typeof(ParticleSystem))]
    public class Emmiter : StaticInstance
    {
        [SerializeField] protected ParticleSystem m_ParticleSystem;
    }
}