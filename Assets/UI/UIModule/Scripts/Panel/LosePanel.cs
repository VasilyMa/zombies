using System;
using UnityEngine;

public class LosePanel : SourcePanel
{
    public RewardData Data { set => GetWindow<LoseWindow>().Data = value; } 

    public override void OnOpen(params Action[] onComplete)
    {
        AddCallback(openLoseWindow);

        base.OnOpen(onComplete);
    }


    void openLoseWindow()
    {
        OpenWindow<LoseWindow>();
    }
}
