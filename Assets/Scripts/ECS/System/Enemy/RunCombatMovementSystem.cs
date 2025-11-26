using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client
{
    sealed class RunCombatMovementSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<EnemyComponent, MovementComponent, TransformComponent, InCombatState>, Exc<DeadComponent, InActionState>> _enemyFilter = default;
        readonly EcsPoolInject<MovementComponent> _movePool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<InCombatState> _combatPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var enemyEntity in _enemyFilter.Value)
            {
                ref var combatComp = ref _combatPool.Value.Get(enemyEntity);
                ref var moveComp = ref _movePool.Value.Get(enemyEntity);
                ref var transformComp = ref _transformPool.Value.Get(enemyEntity);

                if (combatComp.TargetEntity == -1)
                    continue;

                ref var targetTransformComp = ref _transformPool.Value.Get(combatComp.TargetEntity);

                Vector3 direction = targetTransformComp.Transform.position - transformComp.Transform.position;
                direction.y = 0f; // игнорируем вертикаль
                float distance = direction.magnitude;

                if (distance > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transformComp.Transform.rotation = Quaternion.Slerp(
                        transformComp.Transform.rotation,
                        targetRotation,
                        10f * Time.deltaTime
                    );
                }

                if (distance > combatComp.DistanceToAttack)
                {
                    Vector3 moveDir = direction.normalized;
                    transformComp.Transform.position += moveDir * moveComp.CurrentValue * Time.deltaTime;
                }
            }
        }
    }
}
