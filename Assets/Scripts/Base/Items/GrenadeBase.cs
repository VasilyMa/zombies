using Client;
using Leopotam.EcsLite;
using Statement;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeBase", menuName = "Scriptable Objects/GrenadeBase")]
public class GrenadeBase : WeaponBase, ISerializationCallbackReceiver
{
    [Header("View")]
    public Sprite Icon;
    public GameObject MissilePrefab;
    public GameObject MuzzlePrefab;
    public GameObject ExplosionPrefab;

    [Space(5f)]
    [Header("Settings")]
    public float Radius;
    public List<WeaponLevelData> Levels;

    public override void Init(EcsWorld world, BattleState state)
    {
        var levelData = Levels[0];

        if (state.TryGetEntity("player", out int playerEntity))
        {
            var weaponEntity = world.NewEntity();

            ref var holderComp = ref world.GetPool<WeaponHolderComponent>().Get(playerEntity);
            holderComp.WeaponEntities.Add(weaponEntity);

            ref var firepointComp = ref world.GetPool<FirePointComponent>().Get(playerEntity);
            ref var powerComp = ref world.GetPool<PowerComponent>().Get(playerEntity);
            ref var hasteComp = ref world.GetPool<HasteComponent>().Get(playerEntity);

            ref var greandeComp = ref world.GetPool<GrenadeComponent>().Add(weaponEntity);
            greandeComp.Radius = Radius;
            greandeComp.ExplosionPrefab = ExplosionPrefab;

            ref var weaponComp = ref world.GetPool<WeaponComponent>().Add(weaponEntity);
            weaponComp.FirePoint = firepointComp.FirePoint;
            weaponComp.MissilePrefab = MissilePrefab;
            weaponComp.MuzzlePrefab = MuzzlePrefab;
            weaponComp.OwnerEntity = playerEntity;
            weaponComp.Name = Name;
            weaponComp.MaxLevel = Levels.Count;

            ref var attackComp = ref world.GetPool<AttackComponent>().Add(weaponEntity);
            attackComp.Damage = levelData.DamageValue;
            attackComp.Modifier = powerComp.MaxValue;

            ref var rapidFireComp = ref world.GetPool<RapidfireComponent>().Add(weaponEntity);
            rapidFireComp.RapidfireSpeed = levelData.RapidFireValue;
            rapidFireComp.Modifier = hasteComp.MaxValue;

            ref var distanceComp = ref world.GetPool<DistanceComponent>().Add(weaponEntity);
            distanceComp.Value = levelData.Range;

            ref var spreadComp = ref world.GetPool<SpreadComponent>().Add(weaponEntity);
            spreadComp.Angle = levelData.AngleOffset;

            ref var speedComp = ref world.GetPool<SpeedComponent>().Add(weaponEntity);
            speedComp.Value = levelData.MissileSpeedVelocity;

            state.ApplyUpgrade(weaponEntity);
        }
    }

    public override void Upgrade(EcsWorld world, BattleState state)
    {
        if (state.TryGetEntity("player", out int playerEntity))
        {
            ref var holderComp = ref world.GetPool<WeaponHolderComponent>().Get(playerEntity);

            foreach (var weaponEntity in holderComp.WeaponEntities)
            {
                if (world.GetPool<GrenadeComponent>().Has(weaponEntity))
                {
                    Upgrade(world, state, weaponEntity);
                    return;
                }
            }

            Init(world, state);
        }
    }

    public override void Upgrade(EcsWorld world, BattleState state, int entity)
    {
        var weaponEntity = entity;

        ref var weaponComp = ref world.GetPool<WeaponComponent>().Get(weaponEntity);
        int nextLevel = weaponComp.Level + 1;

        if (nextLevel < Levels.Count)
        {
            var levelData = Levels[nextLevel];
            weaponComp.Level = nextLevel;

            ref var attackComp = ref world.GetPool<AttackComponent>().Get(weaponEntity);
            attackComp.Damage = levelData.DamageValue;

            ref var rapidFireComp = ref world.GetPool<RapidfireComponent>().Get(weaponEntity);
            rapidFireComp.RapidfireSpeed = levelData.RapidFireValue;

            ref var distanceComp = ref world.GetPool<DistanceComponent>().Get(weaponEntity);
            distanceComp.Value = levelData.Range;

            ref var spreadComp = ref world.GetPool<SpreadComponent>().Get(weaponEntity);
            spreadComp.Angle = levelData.AngleOffset;

            ref var speedComp = ref world.GetPool<SpeedComponent>().Get(weaponEntity);
            speedComp.Value = levelData.MissileSpeedVelocity;
        }
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
