using System.Collections;
using UnityEngine;

public class LoadingPanel : UIPanel
{
    [SerializeField] private RectTransform spinner;
    [SerializeField] private float speed = 150f;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    private bool isFadingOut = false;

    private void Update()
    {
        spinner.Rotate(0f, 0f, -speed * Time.unscaledDeltaTime);
    }

    public void FadeOutAndClose()
    {
        if (!isOpen || isFadingOut)
        {
            return;
        }
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

    protected override void OnOpened()
    {
        canvasGroup.alpha = 1f;
        isFadingOut = false;
    }
}
