using UnityEngine;

namespace SeveranceStrategy.Buildings.Environment
{
    public sealed class GeyserInstance : StaticInstance
    {
        [SerializeField] private ParticleSystem m_particles;
        [SerializeField] private string m_particleCode;


#if UNITY_EDITOR
        private void Reset() => TryGetComponent(out m_particles);
        private void OnValidate()
        {
            if (m_particles && !string.IsNullOrEmpty(m_particleCode))
            {

            }
        }
#endif
    }
}