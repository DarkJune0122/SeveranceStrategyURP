using UnityEngine;

namespace Dark.ModularUnits
{
    public abstract class Module : MonoBehaviour
    {
        public ModuleSocket[] Sockets => m_sockets;



        private ModuleSocket[] m_sockets;
        public virtual void OnModuleConnected() { }
        public virtual void OnModuleDisconnected() { }
    }
}
