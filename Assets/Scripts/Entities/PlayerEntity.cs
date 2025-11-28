using UnityEngine;

public class PlayerEntity : SourceEntity
{
    public static PlayerEntity Instance { get; private set; }
    public string CurrentStageKey;
    public int LevelID;
    public int StageNumber;
    public int LevelNumber;

    public override void Init()
    { 
        Instance = this;
    }
}
