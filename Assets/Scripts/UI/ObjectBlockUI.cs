using UnityEngine;
using UnityEngine.UI;

namespace SeveranceStrategy.UI
{
    public class ObjectBlockUI : UIElement
    {
        public string Category { get; private set; }
        public string Mode { get; private set; }
        public RawImage Image => m_image;

        [SerializeField] private RawImage m_image;


        public ObjectBlockUI Init(string category, Texture m_icon)
        {
            Category = category;
            m_image.texture = m_icon;
            return this;
        }

        public override void Show() => gameObject.SetActive(true);
        public override void Hide() => gameObject.SetActive(false);
    }
}