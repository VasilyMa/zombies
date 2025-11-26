using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/PlayerConfig")]
public class PlayerConfig : Config
{
    public float Health;
    public float Damage;
    public int Armor;
    public float RapidFire;
    public float MoveSpeed;
    public float BuildSpeed;
    public float BuildHealth;
    public int BuildCost;
    public float Recovery;
    public float ChanceAdditionalShot;

    public List<PlayerLevelData> Levels;

    public override IEnumerator Init()
    {
        yield return null;
    }
}

[System.Serializable]
public class PlayerLevelData
{
    public int Level;
    public int NeededExperience;
}