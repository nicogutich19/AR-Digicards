using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowSceneManager : MonoBehaviour
{
    public static FlowSceneManager Instance;
    [SerializeField] private string arSceneName = "ARExperienceTest";
    [SerializeField] private string mainSceneName = "PanelsFlow";


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public void LoadARExperience()
    {
        SceneManager.LoadScene(arSceneName);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(mainSceneName);
    }

}
