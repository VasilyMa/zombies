using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunRemoveSlowSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<SlowEffectState, DieEvent>> _filter = default;
        readonly EcsPoolInject<SlowEffectState> _slowStatePool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            { 
                _slowStatePool.Value.Del(entity);
            }
        }
    }
}