using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunLevelWinSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<LevelWinEvent>, Exc<LevelLoseEvent>> _filter = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                _world.Value.DelEntity(entity);

                var stageConfig = ConfigModule.GetConfig<StageConfig>();
                  
                int currentStage = PlayerEntity.Instance.StageNumber;
                int currentLevel = PlayerEntity.Instance.LevelNumber;

                var rewardData = new RewardData()
                {
                    KillCount = _state.Value.GetKillCount(),
                    ElapsedTime = _state.Value.GetElapsedTime(),
                    Reward = _state.Value.GetReward(1f)
                };

                if (stageConfig.TryGetNext(currentStage, currentLevel, out StageBase nextStage, out LevelBase nextLevel))
                {
                    PlayerEntity.Instance.StageNumber = nextStage.Number;
                    PlayerEntity.Instance.LevelNumber = nextLevel.Number;
                }

                if (UIModule.TryGetCanvas<BattleCanvas>(out var battleCanvas))
                {
                    if (battleCanvas.TryOpenPanel<WinPanel>(out var winPanel))
                    {
                        winPanel.RewardData = rewardData; 
                    }
                }
            }
        }
    }
}