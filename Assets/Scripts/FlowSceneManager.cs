using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowSceneManager : MonoBehaviour
{
    [SerializeField] private string arSceneName = "ARExperienceTest";
    [SerializeField] private string mainSceneName = "PanelsFlow";

    public void LoadARExperience()
    {
        SceneManager.LoadScene(arSceneName);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
