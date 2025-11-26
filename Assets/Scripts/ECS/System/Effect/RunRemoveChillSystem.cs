using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunRemoveChillSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ChillEffectState, DieEvent>> _filter = default;
        readonly EcsPoolInject<ChillEffectState> _chillStatePool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                _chillStatePool.Value.Del(entity);
            }
        }
    }
}