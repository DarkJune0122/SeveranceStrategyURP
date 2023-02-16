using SeveranceStrategy.Buildings.Processing;
using SeveranceStrategy.TickManagement;
using UnityEngine;

namespace SeveranceStrategy.Buildings
{
    public class HealthObject : StaticObject, ITicker
    {
        [Header(nameof(HealthObject))]
        [SerializeField] protected float m_maxHealth;
        [SerializeField] protected float m_health;
        [SerializeField] protected float m_armour;


        protected virtual void Start() => TickManager.Add(this);
        protected virtual void OnDestroy() => TickManager.Remove(this);

        public virtual void Tick() { }

        public void Damage(float amount)
        {
            m_health -= amount - m_armour;

            if (m_health > 0) return;
            Destroy(gameObject);
        }
        public void Heal(float amount) => m_health = Mathf.Clamp(m_health + amount, 0, m_maxHealth);



#if UNITY_EDITOR
        private void OnValidate() => m_health = m_maxHealth;
#endif
    }
}