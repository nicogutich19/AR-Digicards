using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowSceneManager : MonoBehaviour
{
    private string sceneName = "ARExperienceTest";
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
