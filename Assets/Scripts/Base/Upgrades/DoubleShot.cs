using UnityEngine;
using Client;
using Leopotam.EcsLite;
using Statement;


[CreateAssetMenu(fileName = "DoubleShot", menuName = "Scriptable Objects/Upgrades/DoubleShot")]
public class DoubleShot : UpgradeBase
{
    [Range(1, 100f)] public float ChanceToDoubleShot;

    public override void Apply(EcsWorld world, BattleState state)
    {
        if (state.TryGetEntity("player", out int playerEntity))
        {
            base.Apply(world, state);

            int level = state.GetLevelUpgrade(this);

            ref var doubleComp = ref world.GetPool<UpgradeDoubleShotEvent>().Add(playerEntity);
            doubleComp.Chance += ChanceToDoubleShot * 0.01f;
             
            state.AddUpgrade(this);
        }
    }

    public override void Apply(EcsWorld world, BattleState state, int level, int entity)
    {
        if (!world.GetPool<WeaponComponent>().Has(entity)) return;

        if (!world.GetPool<DoubleShotEffectComponent>().Has(entity)) world.GetPool<DoubleShotEffectComponent>().Add(entity);

        ref var doubleComp = ref world.GetPool<DoubleShotEffectComponent>().Get(entity);
        doubleComp.Chance += ChanceToDoubleShot * 0.01f;
    }

    public override void Upgrade(EcsWorld world, BattleState state)
    {
        if (state.TryGetEntity("player", out int playerEntity))
        {
            base.Apply(world, state);

            int level = state.GetLevelUpgrade(this);

            ref var doubleComp = ref world.GetPool<UpgradeDoubleShotEvent>().Add(playerEntity);
            doubleComp.Chance += ChanceToDoubleShot * 0.01f;

            state.AddUpgrade(this);
        }
    }
}