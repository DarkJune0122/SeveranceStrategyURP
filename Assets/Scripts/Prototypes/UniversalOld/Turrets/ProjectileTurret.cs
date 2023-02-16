using SeveranceStrategy.Projectiles;
using UnityEngine;

namespace SeveranceStrategy.Turrets
{
    public class ProjectileTurret : Turret
    {
        [SerializeField] protected Projectile m_projectile;
        [SerializeField] protected float m_shotCone = 36f;


       /* protected override void Shot()
            => m_projectile.Create(null, transform.position, this, m_target).transform.rotation
             = Quaternion.Euler(0f, 0f, GetDirection() + Random.Range(-m_shotCone, m_shotCone));*/
    }
}