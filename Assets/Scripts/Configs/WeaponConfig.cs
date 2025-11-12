using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Config/WeaponConfig")]
public class WeaponConfig : Config
{
    public List<WeaponBase> WeaponData;

    public override IEnumerator Init()
    {
        yield return null;
    }

    public List<WeaponBase> GetStarterPack()
    {
        var result = new List<WeaponBase>();

        if (WeaponData == null || WeaponData.Count == 0)
            return result;

        // —оздаЄм временный список, чтобы не трогать оригинал
        List<WeaponBase> tempList = new List<WeaponBase>(WeaponData);

        int count = Mathf.Min(3, tempList.Count); // если элементов меньше трЄх Ч берЄм сколько есть

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            result.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex); // чтобы не повтор€лись
        }

        return result;
    }
}
