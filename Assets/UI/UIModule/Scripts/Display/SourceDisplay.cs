using UnityEngine;

public abstract class SourceDisplay : MonoBehaviour
{
    [SerializeField] protected bool IsOpen;
    protected SourcePanel _panel;


    public virtual SourceDisplay Init(SourcePanel panel)
    {
        _panel = panel;
        gameObject.SetActive(IsOpen);
        return this;
    }

    public virtual void OnOpen()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnClose()
    {
        gameObject.SetActive(false);
    }

    public virtual void Dispose()
    {

    }
}
