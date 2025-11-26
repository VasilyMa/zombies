using UnityEngine;

public class PlayerEntity : SourceEntity
{
    public static PlayerEntity Instance { get; private set; }
    public string CurrentStageKey;
    public int LevelID; 

    public override void Init()
    { 
        Instance = this;
    }
}
