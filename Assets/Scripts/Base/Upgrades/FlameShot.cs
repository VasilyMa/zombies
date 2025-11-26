using UnityEngine;
using Client;
using Leopotam.EcsLite;
using Statement;

[CreateAssetMenu(fileName = "FlameShot", menuName = "Scriptable Objects/Upgrades/FlameShot")]
public class FlameShot : UpgradeBase
{
    public GameObject FlamePrefab;
    public float LevelBonus = 1f;
    [Range(0.01f, 1f)] public float DamageValue;
    public float Duration;
    public float Tick;
    public int MaxStack;

    public override void Apply(EcsWorld world, BattleState state)
    {
        if (state.TryGetEntity("player", out int playerEntity))
        {
            base.Apply(world, state);

            int level = state.GetLevelUpgrade(this);

            ref var flameComp = ref world.GetPool<UpgradeFlameEvent>().Add(playerEntity);
            flameComp.Duration = Duration;
            flameComp.Tick = Tick;
            flameComp.Damage = Mathf.Max(DamageValue * (level * LevelBonus), DamageValue);
            flameComp.FlamePrefab = FlamePrefab;

            state.AddUpgrade(this);
        }
    }

    public override void Apply(EcsWorld world, BattleState state, int level, int entity)
    {
        if (!world.GetPool<WeaponComponent>().Has(entity)) return;

        if (!world.GetPool<FlameEffectComponent>().Has(entity)) world.GetPool<FlameEffectComponent>().Add(entity);

        ref var flameComp = ref world.GetPool<FlameEffectComponent>().Get(entity);
        flameComp.Duration = Duration;
        flameComp.Tick = Tick;
        flameComp.Damage = Mathf.Max(DamageValue * (level * LevelBonus), DamageValue);
        flameComp.FlamePrefab = FlamePrefab;
    }

    public override void Upgrade(EcsWorld world, BattleState state)
    { 
        if (state.TryGetEntity("player", out int playerEntity))
        { 
            base.Apply(world, state);

            int level = state.GetLevelUpgrade(this);

            ref var flameComp = ref world.GetPool<UpgradeFlameEvent>().Add(playerEntity);
            flameComp.Duration = Duration;
            flameComp.Tick = Tick;
            flameComp.Damage = Mathf.Max(DamageValue * (level * LevelBonus), DamageValue);
            flameComp.FlamePrefab = FlamePrefab;

            state.AddUpgrade(this);
        }
    }
}
