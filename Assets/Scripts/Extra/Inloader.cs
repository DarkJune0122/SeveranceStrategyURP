using UnityEngine;
using UnityEngine.SceneManagement;

namespace SeveranceStrategy.Extra
{
    public class Inloader : MonoBehaviour
    {
        [SerializeField] private SceneField m_mainMenu;
        private void Awake() => SceneManager.LoadScene(m_mainMenu);
    }
}