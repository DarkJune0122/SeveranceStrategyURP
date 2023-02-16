using UnityEngine;
using UnityEngine.EventSystems;

namespace Dark.ModularUnits.Demo
{
    /// <summary>
    /// Main player base.
    /// </summary>
    public class Iluminum : GameUnit, IModule
    {
        public IModule Parent => null;

        public IModule[] Modules => throw new System.NotImplementedException();

        public Vector2 RelativePosition { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
        }
    }
}
