using Client;
using UnityEngine;
using Leopotam.EcsLite;
using Statement; 

[CreateAssetMenu(fileName = "FreezeShot", menuName = "Scriptable Objects/Upgrades/FreezeShot")]
public class FreezeShot : UpgradeBase
{
    [Range(1f, 100f)] public float Chance;
    public float Duration;

    public override void Apply(EcsWorld world, BattleState state)
    {
        if (state.TryGetEntity("player", out int playerEntity))
        {
            base.Apply(world, state);
            int level = state.GetLevelUpgrade(this);

            ref var flameComp = ref world.GetPool<UpgradeFreezeShotEvent>().Add(playerEntity);
            flameComp.Duration = Duration;
            flameComp.Chance = Chance;

            state.AddUpgrade(this);
        }
    }

    public override void Apply(EcsWorld world, BattleState state, int level, int entity)
    {
        if (!world.GetPool<WeaponComponent>().Has(entity)) return;

        if (!world.GetPool<FreezeEffectComponent>().Has(entity)) world.GetPool<FreezeEffectComponent>().Add(entity);

        ref var freezComp = ref world.GetPool<FreezeEffectComponent>().Get(entity);
        freezComp.Duration = Duration;
        freezComp.Chance = Chance * 0.01f;
    }

    public override void Upgrade(EcsWorld world, BattleState state)
    { 
        if (state.TryGetEntity("player", out int playerEntity))
        {
            base.Apply(world, state);

            int level = state.GetLevelUpgrade(this);

            ref var flameComp = ref world.GetPool<UpgradeFreezeShotEvent>().Add(playerEntity);
            flameComp.Duration = Duration;
            flameComp.Chance = Chance;

            state.AddUpgrade(this);
        }
    }
}