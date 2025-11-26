using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunCombatSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<EnemyComponent, CombatComponent, TransformComponent, AttackComponent>, Exc<InActionState, DeadComponent>> _filter = default;
        readonly EcsPoolInject<CombatComponent> _combatPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<InCombatState> _combatState = default;
        readonly EcsPoolInject<AttackComponent> _attackPool = default;

        int layerMask = LayerMask.GetMask("Player", "Construction");

        Collider[] colliders = new Collider[10]; 

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var combatComp = ref _combatPool.Value.Get(entity);

                combatComp.Delay -= Time.deltaTime;

                if (combatComp.Delay <= 0)
                {
                    _combatPool.Value.Get(entity).Delay = 0.1f;

                    ref var transformComp = ref _transformPool.Value.Get(entity);

                    int hits = Physics.OverlapSphereNonAlloc(transformComp.Transform.position, 10f, colliders, layerMask);

                    int nearestEntity = -1;
                    float nearestSqrDistance = float.MaxValue;

                    for (int i = 0; i < hits; i++)
                    {
                        if (_state.Value.TryGetEntity(colliders[i].name, out int targetEntity))
                        {
                            ref var targetTransformComp = ref _transformPool.Value.Get(targetEntity);

                            Vector3 direction = targetTransformComp.Transform.position - transformComp.Transform.position;
                            float sqrDistance = direction.sqrMagnitude;

                            if (sqrDistance < nearestSqrDistance)
                            {
                                nearestSqrDistance = sqrDistance;
                                nearestEntity = targetEntity;
                            }
                        }
                    }
                     
                    if (nearestEntity != -1)
                    {
                        ref var nearestTarget = ref _transformPool.Value.Get(nearestEntity);
                        Vector3 toTarget = nearestTarget.Transform.position - transformComp.Transform.position;

                        if (!_combatState.Value.Has(entity)) _combatState.Value.Add(entity);
                        ref var attackComp = ref _attackPool.Value.Get(entity);
                        ref var inCombatComp = ref _combatState.Value.Get(entity);
                        inCombatComp.TargetEntity = nearestEntity;
                        inCombatComp.DistanceToAttack = attackComp.Distance;
                    }
                    else
                    {
                        _combatState.Value.Del(entity);
                    }
                }
            }
        }
    }
}