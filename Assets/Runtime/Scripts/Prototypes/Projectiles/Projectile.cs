using UnityEngine;

namespace SeveranceStrategy.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected float m_speed;

        private void FixedUpdate()
        {
            transform.Translate(new Vector3(0, 0, m_speed));
            transform.localPosition = transform.localPosition.Set_Y(transform.localPosition.y - Physics.gravity.y * Time.fixedDeltaTime);
        }
        protected virtual void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.collider.name + " beign hitted!");
            Dispose();
        }

        protected virtual void Dispose() => Destroy(gameObject);
    }
}