using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "StageConfig", menuName = "Config/StageConfig")]
public class StageConfig : Config
{
    public List<StageBase> Stages;
    private Dictionary<string, StageBase> StageConfigs;

    public override IEnumerator Init()
    {
        StageConfigs = new Dictionary<string, StageBase>();

        foreach (var stage in Stages)
        {
            StageConfigs.Add(stage.KeyName, stage);
        }

        yield return null;
    }

    public bool TryGetStage(string key, out StageBase stage)
    {
        if (StageConfigs.TryGetValue(key, out stage))
        {
            return true;
        }

        stage = null; 

        return false;
    }
}
