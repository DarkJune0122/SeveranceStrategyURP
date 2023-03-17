using SeveranceStrategy.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SeveranceStrategy
{
    public class InitializeManager : MonoBehaviour
    {
        public const string TrainingCompletionKey = nameof(TrainingCompletionKey);
        [SerializeField] private Object[] m_dontDestroyOnLoad;
        [SerializeField] private SceneField m_mainMenu;
        [SerializeField] private SceneField m_training;


        private void Awake()
        {
            for (int i = 0; i < m_dontDestroyOnLoad.Length; i++)
            {
                DontDestroyOnLoad(m_dontDestroyOnLoad[i]);
            }

            InitializeAll();

            Debug.Log("Managers were initialized.");
            SceneManager.LoadScene(PlayerPrefs.GetInt(TrainingCompletionKey, 0) == 0 ? m_training : m_mainMenu);
            LoadManager.StopLoading();
        }


        // Static part:
        private static void InitializeAll()
        {
#if DEBUG
            Debug.LogWarning("Initialization started");
#endif

        }
    }
}
