using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunResolveHitSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ResolveHitEvent>> _filter = default;
        readonly EcsPoolInject<DisposeEvent> _disposePool = default; 

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            { 
                _disposePool.Value.Add(entity);
            }
        }
    }
}