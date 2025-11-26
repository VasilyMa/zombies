using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunRemoveBurnSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<BurnEffectState, DieEvent>> _filter = default;
        readonly EcsPoolInject<BurnEffectState> _burnStatePool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                _burnStatePool.Value.Del(entity);
            }
        }
    }
}