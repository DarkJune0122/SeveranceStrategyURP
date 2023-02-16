using System.Collections.Generic;
using UnityEngine;

namespace SeveranceStrategy.UI
{
    public class UIGroupedBlock : MonoBehaviour
    {
        public static readonly Dictionary<ushort, List<UIGroupedBlock>> BlockGroups = new();
        public ushort Group => m_group;


        [SerializeField, Tooltip("Set to 0 to avoid group calculations")] private ushort m_group;


        bool isAligned;
        private void Awake()
        {
            BlockGroups.Clear();
            if (isAligned || m_group == 0) return;
            if (!BlockGroups.TryGetValue(m_group, out List<UIGroupedBlock> blocks))
            {
                blocks = new();
                BlockGroups.Add(m_group, blocks);
            }

            blocks.Add(this);
        }
        private void OnDestroy()
        {
            if (isAligned || m_group == 0) return;
            if (BlockGroups.TryGetValue(m_group, out List<UIGroupedBlock> blocks))
                blocks.Remove(this);
        }
        public static void AlignChilds(GameObject obj)
        {
            UIGroupedBlock[] blocks = obj.GetComponentsInChildren<UIGroupedBlock>(true);
            foreach (UIGroupedBlock block in blocks)
            {
                block.isAligned = true;

                if (!BlockGroups.TryGetValue(block.Group, out List<UIGroupedBlock> blockList))
                {
                    blockList = new();
                    BlockGroups.Add(block.Group, blockList);
                }
                blockList.Add(block);
            }
        }



        public void InvertSelection()
        {
            if (gameObject.activeSelf) Hide();
            else Open();
        }
        public void InvertGroupSelection()
        {
            if (gameObject.activeSelf) Hide();
            else OpenByGroup();
        }
        public virtual void Open() => gameObject.SetActive(true);
        public virtual void OpenByGroup()
        {
            if (BlockGroups.TryGetValue(m_group, out List<UIGroupedBlock> blocks))
            {
                blocks.ForEach((block) => block.Hide());
            }

            Open();
        }
        public virtual void Hide() => gameObject.SetActive(false);
    }
}