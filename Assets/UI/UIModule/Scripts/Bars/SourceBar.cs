using UnityEngine;

public abstract class SourceBar : MonoBehaviour
{
    protected SourcePanel _panel;

    public virtual SourceBar Init(SourcePanel panel)
    {
        _panel = panel;

        return this;
    }
    public void OnClose()
    {

    }

    public virtual void Dispose() { }
}
