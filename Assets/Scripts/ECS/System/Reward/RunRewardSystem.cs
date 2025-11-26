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
        readonly EcsFilterInject<Inc<PlayerComponent>, Exc<PlayerExperienceEvent>> _playerFitlter = default;
        readonly EcsPoolInject<RewardComponent> _rewardPool = default; 
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;
        readonly EcsPoolInject<PlayerExperienceEvent> _playerExpPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var rewardComp = ref _rewardPool.Value.Get(entity);

                if (_state.Value.TryGetEntity("player", out int playerEntity))
                {
                    ref var playerComp = ref _playerPool.Value.Get(playerEntity);

                    playerComp.AddMoney(rewardComp.Reward);

                    playerComp.AddExperience(rewardComp.Experience);

                    foreach (var e in _playerFitlter.Value)
                    {
                        _playerExpPool.Value.Add(e);
                    }
                }
            }
        }
    }
}