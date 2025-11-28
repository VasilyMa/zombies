using System;

public class WinPanel : SourcePanel
{
    public RewardData RewardData { set => GetWindow<WinWindow>().Data = value; }

    public override void OnOpen(params Action[] onComplete)
    {
        AddCallback(openWinWindow);

        base.OnOpen(onComplete);
    }

    void openWinWindow()
    {
        OpenWindow<WinWindow>();
    }
}
