using UnityEngine;
using System.Collections;

public class ScreenshotManager : MonoBehaviour
{
    [SerializeField] private Canvas arExpCanvas;
    [SerializeField] private Canvas captureBrandCanvas;

    private bool isCapturingScreenshot = false;
    private bool isScreenshotCaptured = false;

    private Texture2D capturedTexture;
    private int capturedTextureWidth;
    private int capturedTextureHeight;

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

    private void TextureIsCaptured()
    {
        if (capturedTexture == null)
        {
            Debug.LogError("Error al guardar la captura de pantalla.");
            isScreenshotCaptured = false;
            return;
        }

        if (capturedTexture != null)
        {
            Debug.Log("La captura de pantalla se ha guardado correctamente.");
            isScreenshotCaptured = true;
            ReadingTextureResolution();
        }
    }

    private void ReadingTextureResolution()
    {
        capturedTextureWidth = capturedTexture.width;
        capturedTextureHeight = capturedTexture.height;
        ShowTextureResolution();
    }

    private void ShowTextureResolution()
    {
        if (capturedTextureWidth > 0 && capturedTextureHeight > 0)
        {
            Debug.Log($"Resolución de la captura de pantalla: {capturedTextureWidth}x{capturedTextureHeight}");
        }
        else
        {
            Debug.LogWarning("No se ha capturado ninguna textura para mostrar su resolución.");
            return;
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
            TextureIsCaptured();
        }

        finally
        {
            arExpCanvas.enabled = previousArExpCanvasEnabled;
            captureBrandCanvas.enabled = previousCaptureBrandCanvasEnabled;
            isCapturingScreenshot = false;
        }
    }
}
