using SeveranceStrategy.Projectiles;
using UnityEngine;

namespace SeveranceStrategy.Buildings
{
    public class Turret : DynamicInstance
    {
        public Transform Target
        {
            get => m_target;
            set => SetTarget(value);
        }

        [SerializeField] protected Transform m_target;
        [SerializeField] protected Transform m_head;
        [SerializeField] protected Projectile m_projectile;

        protected virtual void SetTarget(Transform target) => m_target = target;
        protected void Shot(Vector3 target)
        {
            Instantiate(m_projectile.gameObject, transform.parent).transform.LookAt(target);
        }

        private void FixedUpdate()
        {
            if (m_target == null) return;

            m_head.LookAt(EvaluateTarget(m_target.localPosition));
        }
        private Vector3 EvaluateTarget(Vector3 position)
        {
            position.y += (position - m_head.localPosition).magnitude * Physics.gravity.y;
            return position;
        }
    }
}