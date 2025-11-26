using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunRecyclePoolSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<CleanUpEvent, PoolComponent>> _filter = default;
        readonly EcsPoolInject<PoolComponent> _pool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var comp = ref _pool.Value.Get(entity);

                EntityPoolService.Release(comp.KeyName, entity);
            }
        }
    }
}