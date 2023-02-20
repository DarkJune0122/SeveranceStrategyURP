using UnityEngine;

namespace SeveranceStrategy.Buildings
{
    public class StaticInstance : MonoBehaviour
    {
        public virtual void Destroy() => Destroy(gameObject);
    }
}