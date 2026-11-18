using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    public bool isOpen => gameObject.activeSelf;
    public void Open()
    {
        if (!isOpen)
        {
            return;
        }
        OnClosing();
        gameObject.SetActive(true);

    }

    public void Close()
    {

    }

    protected virtual void OnClosing()
    {
    }

    protected virtual void OnOpening()
    {
    }
}
