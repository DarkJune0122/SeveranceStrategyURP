using SeveranceStrategy.Turrets;
using SeveranceStrategy.User;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SeveranceStrategy.ProjectilesOld
{
    /*public class Follower : Projectile
    {
        [SerializeField] protected TrailRenderer m_trailRenderer;
        [SerializeField] protected DestroyableObject m_target;
        [SerializeField] protected float m_minimalRotationSpeed = 20f;
        [SerializeField] protected float m_inaimingTime = 2f;
        [SerializeField] protected float m_expodeDistance = 2f;


        bool isReturning;
        bool isWasAimed;
        float inaimingTime;
        protected override Projectile Initialize(params object[] objs)
        {
            m_turret = objs[0] as Turret;
            m_target = objs[1] as DestroyableObject;
            return this;
        }
        protected override void Update()
        {
            base.Update();
            if (isReturning)
            {
                if (m_turret.Target != null)
                {
                    m_target = m_turret.Target;
                    isReturning = false;
                }
            }
            else
            {
                if (m_target == null)
                {
                    isReturning = true;
                    m_target = m_turret;
                }
            }

            Vector3 delta = new(x: m_target.transform.position.x - transform.position.x,
                                y: m_target.transform.position.y - transform.position.y);
            float targetAngle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg - 90;
            if (inaimingTime > 0) inaimingTime -= Time.deltaTime;
            else
            {
                //Debug.Log(SoftMath.AngleDiffLess(transform.eulerAngles.z, targetAngle, 90));
                if (isWasAimed)
                {
                    //Debug.Log(SoftMath.AngleDiffLarger(transform.eulerAngles.z, targetAngle, 90));
                    if (SoftMath.AngleDiffLarger(transform.eulerAngles.z, targetAngle, 90))
                    {
                        inaimingTime = m_inaimingTime;
                        isWasAimed = false;
                        //Debug.Log(isWasAimed);
                    }
                }
                else
                {
                    if (SoftMath.AngleDiffLess(transform.eulerAngles.z, targetAngle, 90))
                    {
                        isWasAimed = true;
                        //Debug.Log(isWasAimed);
                    }
                }

                transform.rotation = Quaternion.Euler(transform.eulerAngles.Set_Z(Mathf.MoveTowardsAngle(current: transform.eulerAngles.z,
                                     targetAngle, m_minimalRotationSpeed * Time.deltaTime)));
            }


            if ((m_target.transform.localPosition - transform.localPosition).sqrMagnitude < m_expodeDistance)
                Release();
        }
        public override void Release()
        {
            if (m_target && !isReturning && m_target != m_turret)
            {
                m_target.DealDamage(m_damage);

                CameraController.Instance.ShakeFx.Shake(m_shakePower);
            }

            DestroyItself();
        }
        public override void DestroyItself(bool instant = false)
        {
            if (instant)
            {
                PerformDestroy();
            }
            else
            {
                enabled = false;
                CoroutineExecutor.Delay(() => PerformDestroy(), m_trailRenderer.time);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void PerformDestroy()
            {
                base.DestroyItself(instant);
            }
        }
    }*/
}