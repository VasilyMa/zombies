using Client;
using Leopotam.EcsLite;
using Statement;
using UnityEngine;

[CreateAssetMenu(fileName = "SplitHit", menuName = "Scriptable Objects/Upgrades/SplitHit")]
public class SplitHit : UpgradeBase
{
    public float Angle;
    public float DamageValue;
    public int AdditonalMissile;

    public override void Apply(EcsWorld world, BattleState state)
    {
        if (state.TryGetEntity("player", out int playerEntity))
        {
            base.Apply(world, state);

            int level = state.GetLevelUpgrade(this);

            ref var splitComp = ref world.GetPool<UpgradeSplitShotEvent>().Add(playerEntity);
            splitComp.AdditionalMissile = AdditonalMissile;
            splitComp.DamageValue = DamageValue;
            splitComp.Angle = Angle;

            state.AddUpgrade(this);
        }
    }

    public override void Apply(EcsWorld world, BattleState state, int level, int entity)
    {
        if (!world.GetPool<WeaponComponent>().Has(entity)) return;

        if (!world.GetPool<SplitEffectComponent>().Has(entity)) world.GetPool<SplitEffectComponent>().Add(entity);
         
        ref var splitComp = ref world.GetPool<SplitEffectComponent>().Get(entity);
        splitComp.AdditionalMissile = AdditonalMissile;
        splitComp.DamageValue = DamageValue;
        splitComp.Angle = Angle; 
    }

    public override void Upgrade(EcsWorld world, BattleState state)
    {
        if (state.TryGetEntity("player", out int playerEntity))
        {
            base.Apply(world, state);

            int level = state.GetLevelUpgrade(this);

            ref var splitComp = ref world.GetPool<UpgradeSplitShotEvent>().Add(playerEntity);
            splitComp.AdditionalMissile = AdditonalMissile;
            splitComp.DamageValue = DamageValue;
            splitComp.Angle = Angle;

            state.AddUpgrade(this);
        }
    }
}
