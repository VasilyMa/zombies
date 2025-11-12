using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunEnemyMovementSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<EnemyComponent, MovementComponent, TransformComponent>, Exc<DeadComponent, PoolComponent>> _enemyFilter = default;
        readonly EcsPoolInject<MovementComponent> _movePool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var enemyEntity in _enemyFilter.Value)
            {
                ref var moveComp = ref _movePool.Value.Get(enemyEntity);
                ref var transformComp = ref _transformPool.Value.Get(enemyEntity);

                // ѕример простой логики движени€ врага 
                transformComp.Transform.position += transformComp.Transform.forward * moveComp.MoveSpeed * Time.deltaTime;
            }
        }
    }
}
