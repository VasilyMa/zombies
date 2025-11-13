using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunLevelWinSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<LevelWinEvent>> _filter = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                
            }
        }
    }
}