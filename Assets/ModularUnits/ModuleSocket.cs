using Dark.ModularUnits.Demo;
using UnityEngine.EventSystems;

namespace Dark.ModularUnits
{
    public sealed class ModuleSocket : RaycastingBehaviour
    {
        public Module Parent => m_parent;
        public Module Module
        {
            get => m_module;
            set
            {
                if (m_module != null)
                    m_module.OnModuleDisconnected();

                m_module = value;

            }
        }

        private Module m_module;
        private Module m_parent;

        public override void OnPointerDown(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}
