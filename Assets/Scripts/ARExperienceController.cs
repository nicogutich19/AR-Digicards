using UnityEngine;
using Vuforia;

public class ARExperienceController : MonoBehaviour
{
    [SerializeField] private UIPanel loadingPanel;

    public void OnEnable()
    {
        VuforiaApplication.Instance.OnVuforiaStarted += HandleVuforiaStarted;
    }

    public void OnDisable()
    {
        VuforiaApplication.Instance.OnVuforiaStarted -= HandleVuforiaStarted;
    }

    private void HandleVuforiaStarted()
    {
        loadingPanel.Close();
        Debug.Log("Vuforia has been started.");
    }
}
