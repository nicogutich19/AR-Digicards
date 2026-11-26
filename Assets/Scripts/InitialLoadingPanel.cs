using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class InitialLoadingPanel : UIPanel
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.8f;
    [SerializeField] private float displayDuration = 1.8f;

    private bool isFadingOut = false;

    private void Start()
    {
        StartCoroutine(ShowThenFadeRoutine());

    }

    public void FadeOutAndClose()
    {
        if (!isOpen || isFadingOut)
        {
            return;
        }
        isFadingOut = true;
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.unscaledDeltaTime / fadeDuration;
            yield return null;
        }
        Close();
    }

    private IEnumerator ShowThenFadeRoutine()
    {
        yield return new WaitForSecondsRealtime(displayDuration);
        FadeOutAndClose();
    }

    protected override void OnOpened()
    {
        canvasGroup.alpha = 1f;
        isFadingOut = false;
    }
}
