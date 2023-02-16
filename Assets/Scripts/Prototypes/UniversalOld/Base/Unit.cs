using SeveranceStrategy.Blocks;
using SeveranceStrategy.User;
using UnityEngine;

namespace SeveranceStrategy.Units
{
    /// <summary>
    /// Base instance of unit object
    /// </summary>
    public class Unit : DestroyableObject
    {
        [Header(nameof(Unit))]
        [SerializeField] protected float m_deathShakePower = 0.2f;
        [SerializeField] protected float m_speed = 2f;
        [SerializeField] protected Vector2 m_direction = Vector2.zero;


        private Vector2 initialPosition;
        /*public virtual void Init(ushort team) => m_team = team;
        private void Update() => transform.Translate(m_speed * Time.deltaTime * m_direction);
        public override void DestroyItself()
        {
            base.DestroyItself();

            CameraController.Instance.ShakeFx.Shake(m_deathShakePower);
        }*/
    }
}