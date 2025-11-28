using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunLevelFinishSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<LevelComponent, LevelFinishState>> _filter = default;
        readonly EcsFilterInject<Inc<EnemyComponent>, Exc<DeadComponent>> _enemyFilter = default;
        readonly EcsPoolInject<LevelWinEvent> _winEvent = default;
        readonly EcsPoolInject<LevelComponent> _levelPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                if (_enemyFilter.Value.GetEntitiesCount() == 0)
                {
                    ref var levelComp = ref _levelPool.Value.Get(entity);

                    _state.Value.SetElapsedTime(levelComp.ElapsedTime);

                    _winEvent.Value.Add(entity);
                }
            }
        }
    }
}