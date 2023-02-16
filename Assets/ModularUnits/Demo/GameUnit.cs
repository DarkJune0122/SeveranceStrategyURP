using Dark.ModularUnits.Demo.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dark.ModularUnits.Demo
{
    public abstract class GameUnit : RaycastingBehaviour
    {
        /// <summary>
        /// An description of <see cref="GameUnit"/>. 
        /// </summary>
        public string Description => m_description;

        [SerializeField] protected string m_description;

        public override void OnPointerDown(PointerEventData _)
        {
            UIManager.Instance.Descriptor.Setup(m_description);
        }

        /// <summary>
        /// Destroys Unit.
        /// </summary>
        public virtual void Destroy() => Destroy(gameObject);
    }
}