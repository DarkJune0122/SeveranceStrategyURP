using System.Collections.Generic;

namespace SeveranceStrategy.UI
{
    public class BlockCategoryUI : UIElement
    {
        public readonly List<ObjectBlockUI> Blocks = new();
        public string Category { get; set; }


        public BlockCategoryUI Init(string category)
        {
            Category = category;
            return this;
        }

        public override void Show() => gameObject.SetActive(true);
        public override void Hide() => gameObject.SetActive(false);
    }
}