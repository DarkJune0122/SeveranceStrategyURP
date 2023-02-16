using SeveranceStrategy.Blocks;
using System.Collections;
using UnityEngine;

namespace SeveranceStrategy.Turrets
{
    public class Turret : DestroyableObject
    {
        public DestroyableObject Target => m_target;

        [Header(nameof(Turret))]
        [SerializeField] protected SpriteRenderer m_head;
        [SerializeField] protected BulletTrail m_trail;
        [SerializeField] protected Color m_trailColor;
        [SerializeField] protected float m_sqrDetectionRadius;
        [SerializeField] protected float m_sqrShotingRadius;
        [SerializeField] protected float m_damage;
        [SerializeField] protected int m_shotAmount = 1;
        [Space(5f)]
        [SerializeField] protected float m_preshotingDelay;
        [SerializeField] protected float m_shotingDelay;
        [SerializeField] protected float m_betweenShotDelay = 0.1f;
        [SerializeField] protected float m_maxRotationAngle;
        [SerializeField] protected float m_aimedCone = 4f;
        [SerializeField] protected bool m_requireRotation = true;
        [Space(5f)]
        [SerializeField] protected DestroyableObject m_target;


        /*bool aimed;
        float preshotingTime;
        protected override void Awake()
        {
            base.Awake();
            GameManager.Turrets.Add(this);
            if (m_requireRotation)
                StartCoroutine(AimingCoroutine());
            else aimed = true;
            StartCoroutine(ShotRoutine());
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            GameManager.Turrets.Remove(this);
        }


        public virtual void DetectorUpdate()
        {
            if (m_target) return;
            m_target = DetectNearest();
        }
        private IEnumerator AimingCoroutine()
        {
            Vector3 eulers = transform.eulerAngles, delta;
            float angle, targetAngle;
            while (true)
            {
                while (!m_target) yield return null;
                delta = new(x: m_target.transform.localPosition.x - transform.localPosition.x,
                            y: m_target.transform.localPosition.y - transform.localPosition.y);
                targetAngle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
                angle = Mathf.MoveTowardsAngle(current: m_head.transform.eulerAngles.z, targetAngle, maxDelta: m_maxRotationAngle * Time.deltaTime);
                aimed = Mathf.Abs(targetAngle - angle) < m_aimedCone;
                eulers.z = angle;
                m_head.transform.rotation = Quaternion.Euler(eulers);
                yield return null;
            }
        }
        private IEnumerator ShotRoutine()
        {
            float sqrDistance;
            while (true)
            {
                preshotingTime = 0;
                do
                {
                    yield return null;
                    preshotingTime += Time.deltaTime;
                }
                while (preshotingTime < m_preshotingDelay);

                while (!m_target) yield return null;
                while (!aimed) yield return null;
                do
                {
                    sqrDistance = (transform.localPosition - m_target.transform.localPosition).sqrMagnitude;
                    if (aimed)
                    {
                        if (sqrDistance < m_sqrShotingRadius)
                        {
                            if (m_shotAmount == 1) Shot();
                            else for (int i = 0; i < m_shotAmount; i++)
                            {
                                Shot();
                                yield return new WaitForSeconds(m_betweenShotDelay);
                            }
                        }
                    }
                    if (sqrDistance > m_sqrDetectionRadius)
                    {
                        m_target = null;
                        aimed = false;
                    }
                    yield return new WaitForSeconds(m_shotingDelay);
                }
                while (m_target);
            }
        }
        protected virtual void Shot()
        {
            m_target.DealDamage(m_damage);
            m_trail.Create(transform.position, m_target.transform.position, m_trailColor, transform);
        }

        public float GetDirection() => m_head.transform.eulerAngles.z - 90;


        private DestroyableObject DetectObject()
        {
            foreach (Team teams in GameManager.Teams.Values)
            {
                if (teams.TeamID == m_team) continue;
                foreach (DestroyableObject obj in teams.Units)
                {
                    if (InvalidateDistance(transform.localPosition, obj.transform.localPosition, m_sqrDetectionRadius)) continue;
                    return obj;
                }
            }
            return null;
        }
        private DestroyableObject DetectNearest()
        {
            float distance;
            float minDistance = m_sqrDetectionRadius;
            DestroyableObject enemy = null;
            foreach (Team teams in GameManager.Teams.Values)
            {
                if (teams.TeamID == m_team) continue;
                foreach (DestroyableObject obj in teams.Units)
                {
                    distance = (obj.transform.localPosition - transform.localPosition).sqrMagnitude;
                    if (distance < minDistance)
                    {
                        enemy = obj;
                        minDistance = distance;
                    }
                }
            }
            return enemy;
        }
        //Debug.Log((teams.TeamID == m_team) + " " + teams.Units[0].name + " " + teams.TeamID);
        */


        /// <summary>
        /// Validates distance from <paramref name="a"/> to <paramref name="b"/>
        /// </summary>
        /// <returns>True if distance valid</returns>
        private bool ValidateDistance(Vector2 a, Vector2 b, float sqrDistance) => (a - b).sqrMagnitude < sqrDistance;
        /// <summary>
        /// Inverted Validates distance float <paramref name="a"/> to <paramref name="b"/>
        /// </summary>
        /// <returns>True if distance invalid</returns>
        private bool InvalidateDistance(Vector2 a, Vector2 b, float sqrDistance) => (a - b).sqrMagnitude > sqrDistance;
    }
}