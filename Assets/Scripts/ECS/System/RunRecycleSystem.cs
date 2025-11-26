using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunRecycleSystem<T> : IEcsRunSystem where T : struct, IRecyclable
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<T, CleanUpEvent>> _filter = default;
        readonly EcsPoolInject<T> _pool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var comp = ref _pool.Value.Get(entity);
                comp.Cleanup(_world.Value, _state.Value, entity);

                _pool.Value.Del(entity);
            }
        }
    }
}