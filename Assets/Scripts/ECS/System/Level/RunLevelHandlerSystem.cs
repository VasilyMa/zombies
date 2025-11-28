using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunLevelHandlerSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<LevelComponent>, Exc<LevelFinishState, LockState>> _filter = default;
        readonly EcsPoolInject<LevelComponent> _levelPool = default;
        readonly EcsPoolInject<SpawnEvent> _spawnPool = default;
        readonly EcsPoolInject<LevelFinishState> _levelFinishPool = default;

        public void Run (IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var levelComp = ref _levelPool.Value.Get(entity);

                levelComp.ElapsedTime += Time.deltaTime;

                levelComp.SpawnTimer -= Time.deltaTime;

                if (levelComp.SpawnTimer <= 0)
                {
                    levelComp.SpawnTimer = levelComp.LevelData.GetCurrentSpawnInterval(levelComp.ElapsedTime / levelComp.MatchDuration, levelComp.SpawnInterval);

                    int countSpawn = levelComp.LevelData.GetCurrentEnemiesPerSpawn(levelComp.ElapsedTime / levelComp.MatchDuration, levelComp.SpawnCount);

                    float amplifier = levelComp.LevelData.GetCurrentEnemyPowerUp(levelComp.ElapsedTime / levelComp.MatchDuration);

                    float currentTime = levelComp.ElapsedTime;

                    for (global::System.Int32 i = 0; i < countSpawn; i++)
                    { 
                        ref var spawnComp = ref _spawnPool.Value.Add(_world.Value.NewEntity());
                        spawnComp.EnemyBase = levelComp.LevelData.GetCurrentEnemyContainer(currentTime).GetRandomEnemy(); 
                        spawnComp.Amplifier = amplifier;
                        spawnComp.SpawnPoint = levelComp.SpawnPoints[Random.Range(0, levelComp.SpawnPoints.Length)].position;
                    } 
                }

                if (levelComp.ElapsedTime >= levelComp.MatchDuration)
                {
                    _levelFinishPool.Value.Add(entity);
                }

                ObserverEntity.instance.ElapsedTimeChange(levelComp.ElapsedTime);
            }
        }
    }
}
