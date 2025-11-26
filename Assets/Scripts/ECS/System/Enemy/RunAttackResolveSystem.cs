using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunAttackResolveSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<AttackComponent, InCombatState, ResolveAttackEvent, InAttackState, TransformComponent>, Exc<DeadComponent>> _filter = default;
        readonly EcsPoolInject<AttackComponent> _attackPool = default; 
        readonly EcsPoolInject<InAttackState> _inAttackPool = default;
        readonly EcsPoolInject<InCombatState> _combatPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<DamageHandlerComponent> _damagePool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var attackComp = ref _attackPool.Value.Get(entity);
                ref var combatComp = ref _combatPool.Value.Get(entity);
                ref var transfomComp = ref _transformPool.Value.Get(entity);
                ref var targetTransfomComp = ref _transformPool.Value.Get(combatComp.TargetEntity);

                if (Vector3.Distance(targetTransfomComp.Transform.position, transfomComp.Transform.position) <= combatComp.DistanceToAttack + 0.5f)
                {
                    ref var damageComp = ref _damagePool.Value.Get(combatComp.TargetEntity);

                    float damage = attackComp.Damage + attackComp.Damage * attackComp.Modifier;

                    damageComp.DamageValue += damage;
                }

                _inAttackPool.Value.Del(entity);
            }
        }
    }
}