using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunEnemyDeadSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<DieEvent, PoolComponent, EnemyComponent>> _filter = default;
        readonly EcsPoolInject<PoolComponent> _pool = default;
        readonly EcsPoolInject<EnemyComponent> _enemyPool = default;


        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var poolComp = ref _pool.Value.Get(entity);
                ref var enemyComp = ref _enemyPool.Value.Get(entity);

                poolComp.KeyName = enemyComp.EnemyName;

                _state.Value.RemoveEntity(entity.ToString());
            }
        }
    }
}
