using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunDisposeSystem<T> : IEcsRunSystem where T : struct, IDisposable
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<T, DisposeEvent>> _filter = default;
        readonly EcsPoolInject<T> _pool = default;

        public void Run (IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                _pool.Value.Get(entity).Dispose(_world.Value, _state.Value, entity);
            }
        }
    }
}
