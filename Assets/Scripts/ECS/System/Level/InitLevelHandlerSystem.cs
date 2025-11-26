using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Client;
using Statement;
using UnityEngine;
using System.Linq;

namespace Client 
{
    sealed class InitLevelHandlerSystem : IEcsInitSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsPoolInject<LevelComponent> _levelPool = default; 

        public void Init (IEcsSystems systems) 
        {
            var levelData = _state.Value.LevelData;

            var entity = _world.Value.NewEntity();

            ref var levelComp = ref _levelPool.Value.Add(entity);
            levelComp.LevelData = levelData;
            levelComp.MatchDuration = levelData.MaxMatchDuration;
            levelComp.ElapsedTime = 0f;
            levelComp.SpawnInterval = levelData.IntervalSpawn;
            levelComp.SpawnCount = levelData.CountSpawn;
            levelComp.SpawnTimer = levelData.GetCurrentSpawnInterval(0f, levelData.IntervalSpawn); 

            var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

            levelComp.SpawnPoints = spawnPoints.Select(x => x.transform).ToArray();

            _state.Value.AddEntity("level", entity);
        }
    }
}
