using Client;
using Leopotam.EcsLite;
using Statement;
using UnityEngine;

[CreateAssetMenu(fileName = "RecoveryUpgrade", menuName = "Scriptable Objects/Upgrades/RecoveryUpgrade")]
public class RecoveryUpgrade : UpgradeBase
{
    public float RecoveryValue;

    public override void Apply(EcsWorld world, BattleState state)
    {
        base.Apply(world, state);

        if (state.TryGetEntity("player", out int playerEntity))
        {
            base.Apply(world, state);

            int level = state.GetLevelUpgrade(this);

            ref var upgradeComp = ref world.GetPool<UpgradeRecoveryEvent>().Add(playerEntity);
            upgradeComp.BonusValue = RecoveryValue; 

            state.AddUpgrade(this);
        } 
    }

    public override void Apply(EcsWorld world, BattleState state, int level, int entity)
    {

    }

    public override void Upgrade(EcsWorld world, BattleState state)
    {
        if (state.TryGetEntity("player", out int playerEntity))
        {
            base.Apply(world, state);

            int level = state.GetLevelUpgrade(this);

            ref var upgradeComp = ref world.GetPool<UpgradeRecoveryEvent>().Add(playerEntity);
            upgradeComp.BonusValue = RecoveryValue;

            state.AddUpgrade(this);
        }
    }
}
