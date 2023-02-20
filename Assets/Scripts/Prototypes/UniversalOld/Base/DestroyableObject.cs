using SeveranceStrategy.Old;
using UnityEngine;

namespace SeveranceStrategy
{
    public class DestroyableObject : StaticObject
    {
        public float Health => m_maxHealth;

        [Header(nameof(DestroyableObject))]
        [SerializeField] protected float m_maxHealth = 100f;


        protected virtual void Awake() { }
        public override void Instantiate(Vector2 position)
        {
            base.Instantiate(position);
        }
        public class DestroyableInstance : MonoBehaviour
        {
            public ushort Team => m_team;
            [SerializeField] protected ushort m_team;
            [SerializeField] protected float m_maxHealth = 100f;


            [SerializeField] protected float m_health = 100f;
            public void Setup() => GameManager.AddToTeam(m_team, this);
            public void DealDamage(float damage)
            {
                m_health -= damage;

                if (m_health > 0) return;
                Destroy(gameObject); // Other stuff called in OnDestroy()
            }
            public void Heal(float amount)
            {
                m_health += amount;
            }


            protected virtual void Awake() => GameManager.AddToTeam(m_team, this);
            protected virtual void OnDestroy() => GameManager.RemoveObject(this);
        }
    }
}