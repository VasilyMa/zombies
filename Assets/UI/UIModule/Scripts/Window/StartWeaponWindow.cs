 public class StartWeaponWindow : SourceWindow
{
    WeaponSlot[] slots;

    public override SourceWindow Init(SourcePanel panel)
    {
        slots = GetComponentsInChildren<WeaponSlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Init(null);
            slots[i].UpdateView(null);
        }

        return base.Init(panel);
    }

    public override void OnOpen(params object[] data)
    { 
        for (int i = 0; i < slots.Length; i++)
        {
            if (i >= data.Length) break;

            var value = data[i];

            slots[i].UpdateView(value);
        }

        base.OnOpen(data);
    }

    public override void Dispose()
    {

    }
}
