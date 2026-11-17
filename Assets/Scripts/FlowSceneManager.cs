using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowSceneManager : MonoBehaviour
{
    [SerializeField] private string ARSceneName = "ARExperienceTest";
    [SerializeField] private string MainSceneName = "PanelsFlow";
    public void LoadARExperience()
    {
        SceneManager.LoadScene(ARSceneName);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(MainSceneName);
    }
}
