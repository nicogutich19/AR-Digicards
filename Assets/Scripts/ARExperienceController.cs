using System.Collections;
using UnityEngine;
using Vuforia;

public class ARExperienceController : MonoBehaviour
{
    [SerializeField] private UIPanel loadingPanel;
    [SerializeField] private float extraDelay = 2.0f;

    private IEnumerator DelayLoadingPanel()
    {
        yield return new WaitForSecondsRealtime(extraDelay);
        loadingPanel.Close();
    }

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
        StartCoroutine(DelayLoadingPanel());
        Debug.Log("Vuforia has been started.");
    }
}
