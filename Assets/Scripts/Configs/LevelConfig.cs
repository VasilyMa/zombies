using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Config/LevelConfig")]
public class LevelConfig : Config
{
    public List<LevelBase> Levels;

    public override IEnumerator Init()
    { 
        yield return null;
    }
}
