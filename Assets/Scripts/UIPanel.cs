using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    public bool isOpen => gameObject.activeSelf;
    public void Open()
    {
        if (isOpen)
        {
            return;
        }
        gameObject.SetActive(true);
        OnOpened();

    }

    public void Close()
    {
        if (!isOpen)
        {
            return;
        }
        OnClosing();
        gameObject.SetActive(false);

    }

    protected virtual void OnClosing()
    {
    }

    protected virtual void OnOpened()
    {
    }
}
