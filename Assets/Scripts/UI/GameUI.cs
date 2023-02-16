using System.Collections.Generic;
using UnityEngine;

namespace SeveranceStrategy.UI
{
    internal class GameUI : UIElement
    {
        internal static GameUI Instance { get; private set; }
        internal ObjectBlockUI CategoryBlockPrefab => m_categoryBlockPrefab;

        [SerializeField] private ObjectBlockUI m_categoryBlockPrefab;
        [SerializeField] private BlockCategoryUI m_categoryPrefab;
        [SerializeField] private RectTransform m_categoryAnchor;
        private readonly Dictionary<string, BlockCategoryUI> Categories = new();


        BlockCategoryUI lastCategory;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            CreateToolUI();
            Hide();
        }

        private void CreateToolUI()
        {
            foreach (var obj in ModManager.Instance.Objects)
            {
                if (Categories.ContainsKey(obj.Category)) continue;

                Categories.Add(obj.Category, Instantiate(m_categoryPrefab, m_categoryAnchor).Init(obj.Category));
            }

            CreateBlockButtons();
        }
        private void CreateBlockButtons()
        {
            ObjectBlockUI block;
            string lastCategory = null;
            BlockCategoryUI category = null;
            foreach (StaticObject obj in ModManager.Instance.Objects)
            {
                if (lastCategory != obj.Category) // Changes category parent if it's necessary
                {
                    category = Categories[obj.Category]; // Defines required categoy parent
                    lastCategory = obj.Category;
                }

                if (!obj.IsEditorOnly())
                {
                    block = Instantiate(m_categoryBlockPrefab, category.transform);
                    obj.PerformUIBlockActions(block);
                }
            }
        }


        public void UpdateToolUI(string mode) => UpdateBlockButtons(mode);
        private void UpdateBlockButtons(string mode)
        {
            foreach (BlockCategoryUI category in Categories.Values)
            {
                foreach (ObjectBlockUI block in category.Blocks)
                {
                    block.gameObject.SetActive(block.Mode == mode);
                }
            }
        }


        public override void Show() => gameObject.SetActive(true);
        public override void Hide() => gameObject.SetActive(false);
        public void ShowCategory(string category)
        {
            if (lastCategory.Category == category) return;

            if (Categories.TryGetValue(category, out BlockCategoryUI cat))
            {
                cat.Show();
                if (lastCategory)
                    lastCategory.Hide();
                lastCategory = cat;
            }
        }
    }
}