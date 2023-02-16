using Dark.ModularUnits.Demo;
using UnityEngine;

namespace Assets.ModularUnits.Demo
{
    public sealed class Enemy : GameUnit
    {
        public float Health
        {
            get => health;
            set => SetHealth(health: value);
        }

        [SerializeField] private float m_maxHealth;
        [SerializeField] private float m_armor;

        float health;
        private void Awake() => health = m_maxHealth;

        /// <summary>
        /// Deals damage to this unit.
        /// DO NOT use this method for healing, use <see cref="Heal(float)"/> instead.
        /// </summary>
        /// <param name="damage">An damage amount what should be applied to unit health.</param>
        public void DealDamage(float damage)
        {
            damage += m_armor;
            if (damage > 0)
            {
                Health -= damage;
            }
        }
        /// <summary>
        /// Health unit by given hp amount.
        /// </summary>
        public void Heal(float amount)
        {
            health += amount;
            Health = health > m_maxHealth ? health : m_maxHealth;
        }

        /// <summary>
        /// Called from <seealso cref="Health"/>.
        /// </summary>
        private void SetHealth(float health)
        {
            this.health = health;
            if (this.health > 0)
                return;

            Destroy();
        }
    }
}