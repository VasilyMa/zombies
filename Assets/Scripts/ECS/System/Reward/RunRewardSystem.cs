using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunRewardSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<DieEvent, EnemyComponent, RewardComponent>> _filter = default;
        readonly EcsPoolInject<RewardComponent> _rewardPool = default; 

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var rewardComp = ref _rewardPool.Value.Get(entity);
            }
        }
    }
}