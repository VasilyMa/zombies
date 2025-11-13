using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class InitWallObjectSystem : IEcsInitSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsPoolInject<WallComponent> _wallPool = default;
        readonly EcsPoolInject<BuildProcessState> _buildProcessPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;

        public void Init (IEcsSystems systems) 
        {
            var walls = GameObject.FindObjectsByType<WallZone>(FindObjectsSortMode.None);

            foreach (var wall in walls)
            {
                var wallEntity = _world.Value.NewEntity();

                wall.Entity = wallEntity;
                wall.TriggerEntered += StartBuild;
                wall.TriggerExited += StopBuild;

                _wallPool.Value.Add(wallEntity).TargetProgress = 5;
                ref var transformComp = ref _transformPool.Value.Add(wallEntity);
                transformComp.Transform = wall.wallObject.transform;
                transformComp.Transform.gameObject.SetActive(false);
            }
        }

        void StartBuild(int entity, Collider collider)
        {
            if(!_buildProcessPool.Value.Has(entity)) 
                _buildProcessPool.Value.Add(entity).InitialDelay = 1f;
        }

        void StopBuild(int entity, Collider collider)
        {
            if(_buildProcessPool.Value.Has(entity)) 
                _buildProcessPool.Value.Del(entity);
        }
    }
}
