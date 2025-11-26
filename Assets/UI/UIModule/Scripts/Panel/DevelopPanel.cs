using System;
using UnityEngine;

public class DevelopPanel : SourcePanel
{
    public override void OnOpen(params Action[] onComplete)
    {
        OpenLayout<DevelopLayout>();

        base.OnOpen(onComplete);
    }
}
