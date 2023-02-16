using System.Collections.Generic;
using UnityEngine;

namespace SeveranceStrategy
{
    public class ModManager : MonoBehaviour
    {
        internal static ModManager Instance { get; private set; }
        public static readonly List<string> Categories = new();

        internal List<StaticObject> Objects => m_objects;
        [SerializeReference] private List<StaticObject> m_objects = new();


        private void Awake()
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        //private void LoadAdressables() { }
    }
}