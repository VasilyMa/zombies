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

    public bool TryGetNext(int stageNumber, int levelNumber, out StageBase nextStage, out LevelBase nextLevel)
    {
        nextStage = null;
        nextLevel = null;

        // 1) Находим текущий стейдж
        StageBase currentStage = Stages.Find(s => s.Number == stageNumber);
        if (currentStage == null)
            return false;

        // 2) Ищем следующий уровень внутри текущего стейджа
        int targetLevel = levelNumber + 1;

        LevelBase foundLevel = currentStage.Levels.Find(l => l.Number == targetLevel);
        if (foundLevel != null)
        {
            nextStage = currentStage;
            nextLevel = foundLevel;
            return true;
        }

        // 3) Следующего уровня нет → ищем следующий стейдж
        int nextStageNumber = stageNumber + 1;

        StageBase foundStage = Stages.Find(s => s.Number == nextStageNumber);
        if (foundStage != null && foundStage.Levels.Count > 0)
        {
            nextStage = foundStage;
            nextLevel = foundStage.Levels[0]; // первый уровень
            return true;
        }

        // 4) Ничего нет
        return false;
    }
}
