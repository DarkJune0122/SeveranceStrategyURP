using UnityEngine;
using UnityEngine.UI;

namespace Dark.ModularUnits.Demo.UI
{
    public sealed class Descriptor : MonoBehaviour
    {
        [SerializeField] private Text m_memo;

        public void Setup(string description)
        {
            m_memo.text = description;
        }
    }
}