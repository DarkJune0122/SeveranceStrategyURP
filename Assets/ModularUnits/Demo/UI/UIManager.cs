using UnityEngine;

namespace Dark.ModularUnits.Demo.UI
{
    public sealed class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public Descriptor Descriptor => m_descriptor;

        [SerializeField] private Descriptor m_descriptor;


        private void Awake() => Instance = this;
    }
}