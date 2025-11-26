using Client;
using Statement;
using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "FractionShot", menuName = "Scriptable Objects/Upgrades/FractionShot")]
public class FractionShot : UpgradeBase
{
    public float Radius;
    public float DamageValue;
    public float LevelBonus;

    public override void Apply(EcsWorld world, BattleState state)
    {
        if (state.TryGetEntity("player", out int playerEntity))
        {
            base.Apply(world, state);

            int level = state.GetLevelUpgrade(this);

            ref var upgradeComp = ref world.GetPool<UpgradeFractionShotEvent>().Add(playerEntity);
            upgradeComp.Radius = Radius;
            upgradeComp.DamageValue = Mathf.Max(DamageValue * (level * LevelBonus), DamageValue); 

            state.AddUpgrade(this);
        }
    }

    public override void Apply(EcsWorld world, BattleState state, int level, int entity)
    {
        if (!world.GetPool<WeaponComponent>().Has(entity)) return;

        if (!world.GetPool<FractionEffectComponent>().Has(entity)) world.GetPool<FractionEffectComponent>().Add(entity);

        ref var fractionComp = ref world.GetPool<FractionEffectComponent>().Get(entity);
        fractionComp.Radius = Radius;
        fractionComp.DamageValue = Mathf.Max(DamageValue * (level * LevelBonus), DamageValue);
    }

    public override void Upgrade(EcsWorld world, BattleState state)
    {
        if (state.TryGetEntity("player", out int playerEntity))
        {
            base.Apply(world, state);

            int level = state.GetLevelUpgrade(this);

            ref var upgradeComp = ref world.GetPool<UpgradeFractionShotEvent>().Add(playerEntity);
            upgradeComp.Radius = Radius;
            upgradeComp.DamageValue = Mathf.Max(DamageValue * (level * LevelBonus), DamageValue); 

            state.AddUpgrade(this);
        }
    }
}