using UnityEngine;
using System.Collections;

public class ScreenshotManager : MonoBehaviour
{
    [SerializeField] private Canvas arExpCanvas;
    [SerializeField] private Canvas captureBrandCanvas;

    private bool isCapturingScreenshot = false;

    private Texture2D capturedTexture;

    public void RequestScreenshotCapture()
    {

        if (this.arExpCanvas == null || this.captureBrandCanvas == null)
        {
            Debug.LogError("Uno o ambos canvas no han sido asignados.");
            return;
        }

        if (isCapturingScreenshot == true)
        {
            Debug.LogWarning("Captura de pantalla en progreso.");
            return;
        }

        if (isCapturingScreenshot == false)
        {
            Debug.Log("Iniciando captura de pantalla...");
            isCapturingScreenshot = true;
            StartCoroutine(CaptureScreenshotRoutine());
        }
    }

    private IEnumerator CaptureScreenshotRoutine()
    {
        bool previousArExpCanvasEnabled = this.arExpCanvas.enabled;
        bool previousCaptureBrandCanvasEnabled = this.captureBrandCanvas.enabled;

        try
        {
            arExpCanvas.enabled = false;
            captureBrandCanvas.enabled = true;
            yield return new WaitForEndOfFrame();
            capturedTexture = ScreenCapture.CaptureScreenshotAsTexture();

        }

        finally
        {
            arExpCanvas.enabled = previousArExpCanvasEnabled;
            captureBrandCanvas.enabled = previousCaptureBrandCanvasEnabled;
            isCapturingScreenshot = false;
        }

    }
}
