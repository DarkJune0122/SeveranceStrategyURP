using SeveranceStrategy.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SeveranceStrategy
{
    public sealed class InloadRedirect : MonoBehaviour
    {
        [SerializeField] private SceneField m_inloadScene;

        private void Awake()
        {
            if (GameManager.Instance) return;

            SceneManager.LoadScene(m_inloadScene);
        }
    }
}