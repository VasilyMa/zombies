using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client
{
    sealed class RunPlayerLevelUpdateSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<PlayerExperienceEvent, PlayerComponent>> _filter = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;
        readonly EcsPoolInject<PlayerLevelUpEvent> _levelUpEvent = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var player = ref _playerPool.Value.Get(entity);

                var playerConfig = ConfigModule.GetConfig<PlayerConfig>();
                var levels = playerConfig.Levels;

                if (levels == null || levels.Count == 0)
                    continue;

                int currentLevel = player.Level;
                int exp = player.Experience;

                // Проверяем, что следующий индекс существует
                if (currentLevel < 0 || currentLevel >= levels.Count)
                    continue; // уровень вне диапазона — ничего не делаем

                // Проверяем, есть ли следующий уровень
                int nextLevelIndex = currentLevel + 1;

                if (nextLevelIndex >= levels.Count)
                    continue; // игрок на максимальном уровне

                int needed = levels[currentLevel].NeededExperience;

                // Если опыта достаточно — повышаем уровень
                if (exp >= needed)
                {
                    player.AddNeedeExperience(levels[nextLevelIndex].NeededExperience);
                    player.AddLevel(1);

                    _levelUpEvent.Value.Add(entity);
                }
            }
        }
    }
}
