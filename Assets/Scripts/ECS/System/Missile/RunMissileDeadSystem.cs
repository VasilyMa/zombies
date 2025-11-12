using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client {
    sealed class RunMissileDeadSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<DieEvent, PoolComponent, MissileComponent>> _filter = default;
        readonly EcsPoolInject<PoolComponent> _pool = default;
        readonly EcsPoolInject<MissileComponent> _missilePool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var poolComp = ref _pool.Value.Get(entity);
                ref var missileComp = ref _missilePool.Value.Get(entity);

                poolComp.KeyName = missileComp.KeyName;
            }
        }
    }
}
