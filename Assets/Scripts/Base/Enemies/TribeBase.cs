using Client;
using Leopotam.EcsLite;
using Statement;
using UnityEngine;

[CreateAssetMenu(fileName = "TribeBase", menuName = "Scriptable Objects/TribeBase")]
public class TribeBase : EnemyBase
{
    public AbilityBase ability;

    public override int Init(EcsWorld world, BattleState state, ref SpawnEvent spawnComp)
    {
        var entity = world.NewEntity();

        world.GetPool<PoolComponent>().Add(entity).KeyName = EnemyName;

        var entityKey = entity.ToString();

        var instance = GameObject.Instantiate(Prefab, spawnComp.SpawnPoint, Quaternion.identity);

        instance.gameObject.name = entityKey;

        float amplifier = spawnComp.Amplifier;

        ref var attackComp = ref world.GetPool<AttackComponent>().Add(entity);
        attackComp.Damage = Attack + (Attack * amplifier);
        attackComp.Distance = Range;
        attackComp.Delay = Delay;
        ref var moveComp = ref world.GetPool<MovementComponent>().Add(entity);
        moveComp.Init(MoveSpeed + (MoveSpeed * amplifier));
        ref var healthComp = ref world.GetPool<HealthComponent>().Add(entity);
        healthComp.Init(Health * (Health * amplifier));
        ref var rewardComp = ref world.GetPool<RewardComponent>().Add(entity);
        rewardComp.Reward = Reward;
        rewardComp.Experience = Experience;
        ref var danageComp = ref world.GetPool<DamageHandlerComponent>().Add(entity);
        ref var enemyComp = ref world.GetPool<EnemyComponent>().Add(entity);
        enemyComp.EnemyName = EnemyName;

        ref var transformComp = ref world.GetPool<TransformComponent>().Add(entity);
        transformComp.Transform = instance.transform;
        transformComp.Transform.position = spawnComp.SpawnPoint;
        transformComp.Transform.rotation = new Quaternion(0, 180, 0, 0);
        transformComp.Transform.gameObject.SetActive(true);

        ref var animateComp = ref world.GetPool<AnimateComponent>().Add(entity);
        animateComp.Animator = transformComp.Transform.GetComponentInChildren<Animator>();

        ref var abilityComp = ref world.GetPool<AbilityComponent>().Add(entity);
        abilityComp.AbilityBase = ability;

        ref var cooldownComp = ref world.GetPool<CooldownComponent>().Add(entity);
        cooldownComp.RemainingTime = 2;

        state.AddEntity(entityKey, entity);

        return entity;
    } 
}
