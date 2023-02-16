using SeveranceStrategy.Turrets;
using System.Collections;
using UnityEngine;

namespace SeveranceStrategy.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer m_spriteRenderer;
        [SerializeField] protected float m_lifetime = 100f;
        [SerializeField] protected float m_speed = 10f; // Speed in units per second
        [SerializeField] protected float m_damage = 10f;
        [SerializeField] protected float m_penetration = 0f;
        [SerializeField] protected float m_shakePower = 0f;
        protected Turret m_turret;


        private float time;
        public virtual Projectile Create(Transform parent, Vector2 source, params object[] objs) => Instantiate(gameObject, source, Quaternion.identity, parent).GetComponent<Projectile>().Initialize(objs);
        protected virtual Projectile Initialize(params object[] objs) => this;
        protected virtual void Update()
        {
            time += Time.deltaTime;

            if (time > m_lifetime) DestroyItself();
            else transform.Translate(new(0, m_speed * Time.deltaTime, 0), Space.Self);
        }
        public abstract void Release();
        public virtual void DestroyItself(bool instant = false) => Destroy(gameObject);

#if UNITY_EDITOR
        private void Reset()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }
#endif
    }
}