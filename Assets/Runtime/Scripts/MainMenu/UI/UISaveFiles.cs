using SeveranceStrategy.Game;
using SeveranceStrategy.IO;
using SeveranceStrategy.UI.Structs;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SeveranceStrategy.UI
{
    public class UISaveFiles : MonoBehaviour
    {
        [SerializeField] private UISaveFile m_filePrefab;
        [SerializeField] private VerticalLayoutGroup m_layoutGroup;

        [Header("Preview")]
        [SerializeField] private RawImage m_preview;
        [SerializeField] private GameObject m_uiGroup;
        [SerializeField] private Text m_name;
        [SerializeField] private Text m_description;
        [SerializeField] private Text m_recap;
        [SerializeField] private Text m_path;
        [Space(3f)]
        [SerializeField] private Texture2D m_placeholder;

        private UISaveFile[] m_saveFiles = new UISaveFile[0];
        private SaveFile m_previewFile;
        private void OnEnable()
        {
            PerformSelect(null);
            string[] paths = Directory.GetFiles($"{Json.ClientPath}{SaveFile.Root}", $"*{SaveFile.Extention}");
            Resize(paths.Length);
            for (int i = 0; i < paths.Length; i++)
            {
                m_saveFiles[i].Setup(SaveFile.Load(paths[i]), PerformSelect);
            }

            void Resize(int toLength)
            {
                UISaveFile[] files = new UISaveFile[toLength];
                toLength = files.Length - m_saveFiles.Length;
                if (toLength > 0)
                {
                    Miscellaneous.Copy(m_saveFiles, files);
                    for (toLength = m_saveFiles.Length; toLength < files.Length; toLength++)
                        files[toLength] = Instantiate(m_filePrefab, m_layoutGroup.transform);
                }
                else if (toLength < 0)
                {
                    for (toLength = m_saveFiles.Length; toLength < files.Length; toLength++)
                        Destroy(m_saveFiles[toLength].gameObject);
                }
                m_saveFiles = files;
            }
        }
        private void OnDisable() => m_uiGroup.SetActive(false);

        private void PerformSelect(SaveFile file)
        {
            m_previewFile = file;

            bool enabled = file != null;
            m_path.enabled = Settings.ShowSaveFilePath.Value && enabled;
            m_uiGroup.SetActive(enabled);

            if (enabled == false)
            {
                m_preview.texture = m_placeholder;
                return;
            }
            m_name.text = file.name;
            m_description.text = file.description;
            m_recap.text = file.recap;
            m_path.text = file.path;
            //m_preview.texture = m_placeholder;
        }

        public void LoadSaveFile()
        {
            if (m_previewFile == null) return;
            GameManager.LoadGame(m_previewFile);
        }

        public void DeleteSaveFile() => throw new NotImplementedException();
    }
}