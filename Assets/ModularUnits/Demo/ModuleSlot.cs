using UnityEngine.EventSystems;

namespace Dark.ModularUnits.Demo
{
    public class ModuleSlot : RaycastingBehaviour
    {
        public IModule Module
        {
            get => module;
            set => module = value;
        }


        private IModule module;
        public override void OnPointerDown(PointerEventData eventData)
        {

        }
    }
}