using UnityEngine;

namespace Client 
{
    struct LevelComponent 
    {
        public LevelBase LevelData;
        public Transform[] SpawnPoints;
        public float ElapsedTime;
        public float SpawnTimer;
        public float SpawnInterval;
        public float MatchDuration; 
        public int SpawnCount;
    }
}
