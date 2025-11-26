using Leopotam.EcsLite;
using Statement;
using UnityEngine;
using Client;

[CreateAssetMenu(fileName = "StatUpgrade", menuName = "Scriptable Objects/Upgrades/StatUpgrade")]
public class StatBonus : UpgradeBase
{
    public float BuildProcessBonus;
    public float BuildHealthBonus;
    public float PlayerHealthBonus;
    public float PlayerDamageBonus;
    public float PlayerAttackSpeedBonus;
    public float PlayerHealthRegenerationBonus;
    public float PlayerMoveSpeedBonus;

    public override void Apply(EcsWorld world, BattleState state)
    {
        if (state.TryGetEntity("player", out int playerEntity))
        {
            base.Apply(world, state);

            ref var upgradeComp = ref world.GetPool<UpgradeStatEvent>().Add(playerEntity);
            upgradeComp.BuildProcessBonus = BuildProcessBonus;
            upgradeComp.BuildHealthBonus = BuildHealthBonus;
            upgradeComp.PlayerHealthBonus = PlayerHealthBonus;
            upgradeComp.PlayerDamageBonus = PlayerDamageBonus;
            upgradeComp.PlayerAttackSpeedBonus = PlayerAttackSpeedBonus;
            upgradeComp.PlayerHealthRegenerationBonus = PlayerHealthRegenerationBonus;
            upgradeComp.PlayerMoveSpeedBonus = PlayerMoveSpeedBonus;
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

            ref var upgradeComp = ref world.GetPool<UpgradeStatEvent>().Add(playerEntity);
            upgradeComp.BuildProcessBonus = BuildProcessBonus;
            upgradeComp.BuildHealthBonus = BuildHealthBonus;
            upgradeComp.PlayerHealthBonus = PlayerHealthBonus;
            upgradeComp.PlayerDamageBonus = PlayerDamageBonus;
            upgradeComp.PlayerAttackSpeedBonus = PlayerAttackSpeedBonus;
            upgradeComp.PlayerHealthRegenerationBonus = PlayerHealthRegenerationBonus;
            upgradeComp.PlayerMoveSpeedBonus = PlayerMoveSpeedBonus;
        }
    }
}