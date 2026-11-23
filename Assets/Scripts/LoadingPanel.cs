using UnityEngine;

public class LoadingPanel : UIPanel
{
    [SerializeField] private RectTransform spinner;
    [SerializeField] private float speed = 150f;

    private void Update()
    {
        spinner.Rotate(0f, 0f, -speed * Time.unscaledDeltaTime);
    }
}
