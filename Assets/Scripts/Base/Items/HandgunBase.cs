using Client;
using Leopotam.EcsLite;
using Statement;
using System.Collections.Generic; 
using UnityEngine;

[CreateAssetMenu(fileName = "Handgun", menuName = "Scriptable Objects/Handgun")]
public class HandgunBase : WeaponBase, ISerializationCallbackReceiver
{
    [Header("View")]
    public string Name;
    public Sprite Icon;
    public GameObject MissilePrefab;
    public GameObject MuzzlePrefab;

    [Space(5f)]
    [Header("Settings")]
    public List<WeaponLevelData> Levels;

    public override void Init(EcsWorld world, BattleState state)
    {
        var levelData = Levels[0];

        if (state.TryGetEntity("player", out int playerEntity))
        {
            var weaponEntity = world.NewEntity();

            ref var firepointComp = ref world.GetPool<FirePointComponent>().Get(playerEntity);
            ref var handgunComp = ref world.GetPool<HandgunComponent>().Add(weaponEntity);

            ref var weaponComp = ref world.GetPool<WeaponComponent>().Add(weaponEntity);
            weaponComp.FirePoint = firepointComp.FirePoint;
            weaponComp.MissilePrefab = MissilePrefab;
            weaponComp.MuzzlePrefab = MuzzlePrefab;
            weaponComp.OwnerEntity = playerEntity;

            ref var attackComp = ref world.GetPool<AttackComponent>().Add(weaponEntity);
            attackComp.Damage = levelData.DamageValue;

            ref var rapidFireComp = ref world.GetPool<RapidfireComponent>().Add(weaponEntity);
            rapidFireComp.RapidfireSpeed = levelData.RapidFireValue;

            ref var distanceComp = ref world.GetPool<DistanceComponent>().Add(weaponEntity);
            distanceComp.Value = levelData.Range;

            ref var spreadComp = ref world.GetPool<SpreadComponent>().Add(weaponEntity);
            spreadComp.Angle = levelData.AngleOffset;

            ref var speedComp = ref world.GetPool<SpeedComponent>().Add(weaponEntity);
            speedComp.Value = levelData.MissileSpeedVelocity;
        }
    }

    public override void Upgrade(EcsWorld world, BattleState state, int currentLevels)
    { 

    }

    public override string GetDescription(int level)
    { 
        if (Levels == null || Levels.Count == 0) return string.Empty;
        if (level < 0 || level >= Levels.Count) return string.Empty;
        return Levels[level].Description;
    }

    public void OnAfterDeserialize()
    { 
    }

    public void OnBeforeSerialize()
    {
        if (Levels == null || Levels.Count == 0) return;

        foreach (var level in Levels)
        { 
            if (level.RapidFireValue != 0)
            {
                level.DPS = level.DamageValue / level.RapidFireValue;
            }
        }
    }

}

[System.Serializable]
public class WeaponLevelData
{
    public string Description;
    [ReadOnlyInspector] public float DPS;
    public float DamageValue;
    public float RapidFireValue;
    public float Range;
    public float MissileSpeedVelocity;
    public int MissileCount;
    public float AngleOffset;  
}