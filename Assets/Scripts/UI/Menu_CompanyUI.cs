using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_CompanyUI : MonoBehaviour
{
    [SerializeField] private SceneField m_storyScene;


    public void LoadCompany() => SceneManager.LoadScene(m_storyScene);
}
