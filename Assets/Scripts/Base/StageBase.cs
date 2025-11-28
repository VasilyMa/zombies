using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageBase", menuName = "Scriptable Objects/StageBase")]
public class StageBase : ScriptableObject
{
    public int Number;
    public string KeyName;
    public string Name;
    public string Description;
    public Sprite Icon;
    public List<LevelBase> Levels;

    public LevelBase GetLevelByID(int id)
    {
        return Levels[id];
    }

}
