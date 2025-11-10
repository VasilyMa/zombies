using UnityEngine;

public abstract class SourceLayout : MonoBehaviour
{
    [SerializeField] protected bool IsOpen;
    protected SourcePanel _panel;
    protected SourceSlot[] _slots;

    public virtual SourceLayout Init(SourcePanel panel)
    {
        _panel = panel;

        var slots = GetComponentsInChildren<SourceSlot>(true);
        _slots = new SourceSlot[slots.Length];

        for (int i = 0; i < slots.Length; i++) _slots[i] = slots[i].Init(this);

        gameObject.SetActive(IsOpen);

        return this;
    }
    public virtual void OnInject()
    {

    }
    public virtual T GetSourcePanel<T>() where T : SourcePanel
    {
        return _panel as T;
    }
    public virtual void ResetSlots()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].UpdateView();
        }
    }
    public virtual void CloseIt<T>() where T : SourceLayout
    {
        _panel.CloseLayout<T>();
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
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].Dispose();
        }
    }
}
