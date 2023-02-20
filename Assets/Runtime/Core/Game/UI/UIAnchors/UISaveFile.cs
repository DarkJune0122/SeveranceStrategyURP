using SeveranceStrategy.IO;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeveranceStrategy.UI.Structs
{
    public sealed class UISaveFile : MonoBehaviour
    {
        [SerializeField] private Text m_name;
        [SerializeField] private Text m_recap;

        private SaveFile m_file;
        private Action<SaveFile> m_pressCallback;
        public void Setup(SaveFile file, Action<SaveFile> pressCallback)
        {
            m_file = file;
            m_name.text = file.name;
            m_recap.text = file.recap;
            m_pressCallback = pressCallback;
        }

        /// <summary>
        /// Call this method to 
        /// </summary>
        public void Button_SetPreview() => m_pressCallback?.Invoke(m_file);
    }
}