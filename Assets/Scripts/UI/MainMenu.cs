using SeveranceStrategy.UI;
using UnityEngine;

namespace SeveranceStrategy.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Transform m_backgroundAnchor;


        private void Awake() => UIGroupedBlock.AlignChilds(transform.parent.gameObject);
        public void PerformApplicationQuit() => Application.Quit();
    }
}