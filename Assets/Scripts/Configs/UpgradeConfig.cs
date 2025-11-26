using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Config/UpgradeConfig")]
public class UpgradeConfig : Config
{
    public List<UpgradeBase> Upgrades;

    public override IEnumerator Init()
    {
        yield return null;
    }

    public UpgradeBase GetUpgrade(string name)
    {
        return Upgrades.FirstOrDefault(x => x.Name == name);
    }

    /// <summary>
    /// Возвращает случайные апгрейды, исключая уже имеющиеся.
    /// </summary>
    public List<IUpgrade> GetRandomUpgrades(int count, string[] exclude)
    {
        List<UpgradeBase> available = new();

        // собираем доступные
        foreach (var upg in Upgrades)
        {
            if (!exclude.Contains(upg.Name))
                available.Add(upg);
        }

        // если апгрейдов меньше чем нужно — отдаём все
        if (available.Count <= count)
            return new List<IUpgrade>(available);

        // перемешиваем
        for (int i = 0; i < available.Count; i++)
        {
            int rand = Random.Range(i, available.Count);
            (available[i], available[rand]) = (available[rand], available[i]);
        }

        // создаём результат
        return available.GetRange(0, count).ConvertAll(x => (IUpgrade)x);
    }

    public UpgradeBase GetRandomUpgrade()
    {
        if (Upgrades == null || Upgrades.Count == 0)
            return null;

        int index = Random.Range(0, Upgrades.Count);
        return Upgrades[index];
    }
}
