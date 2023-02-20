using SeveranceStrategy.Modularity;
using SeveranceStrategy.UI;
using UnityEngine;
using Object = System.Object;

namespace SeveranceStrategy.Old
{
    public class StaticObject : Object
    {
        public string Category => m_category;

        [Header(nameof(StaticObject))]
        public SpriteRenderer m_base;
        public string m_category = "blocks";
        public string[] m_modes = { "allways" };
        public Texture m_icon;
        public SizeSocket[] Sockets;





        // Between-scripts interactions
        public virtual void PerformUIBlockActions(ObjectBlockUI block) => block.Init(m_category, m_icon);

        public bool IsEditorOnly() => m_modes.Length < 1 || m_modes[0] == "editor";
        /// <summary>
        /// Returns TRUE if object can be used in this mode
        /// </summary>
        public bool IsModeValid(string mode)
        {
            for (int i = 0; i < m_modes.Length; i++)
                if (m_modes[i].Equals(mode))
                    return true;

            return false;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            m_category = m_category.ToLower();
            for (int i = 0; i < m_modes.Length; i++)
            {
                m_modes[i] = m_modes[i].ToLower();
            }
        }
#endif


        public virtual void Instantiate(Vector2 position) { }
        public class StaticInstance : MonoBehaviour
        {
            public virtual void DestroyItself() => Destroy(gameObject);
        }
    }
}
