using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWindow : SourceWindow
{
    public Action OnFinish;
    public List<IUpgrade> Data;
    UpgradeSlot[] _slots;

    public override SourceWindow Init(SourcePanel panel)
    {
        _slots = GetComponentsInChildren<UpgradeSlot>(true);

        foreach (var slot in _slots)
        {
            slot.Init(null);
            slot.UpdateView(null);
        }

        return base.Init(panel);
    }  

    public void InvokeRewardList(List<IUpgrade> data)
    {
        Data = data;

        for (int i = 0; i < _slots.Length; i++)
        {
            if (i >= data.Count) break;

            var value = data[i];

            _slots[i].UpdateView(value);
        } 
    }

    public override void OnClose()
    {
        base.OnClose();

        Time.timeScale = 1.0f;
    }

    public override void OnOpen(params object[] data)
    {
        Time.timeScale = 0f;

        base.OnOpen(data);
    } 

    public override void Dispose()
    {

    }  
}
