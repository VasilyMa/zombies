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
        readonly EcsPoolInject<EngineComponent> _enginePool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<DamageHandlerComponent> _damageHandlerPool = default;

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

                _state.Value.AddEntity(wallEntity.ToString(), wallEntity);

                wall.gameObject.name = wallEntity.ToString();

                _damageHandlerPool.Value.Add(wallEntity);
            }
        }

        void StartBuild(int entity, Collider collider)
        {
            if (!_buildProcessPool.Value.Has(entity))
            {
                if (_state.Value.TryGetEntity("player", out int playerEntity))
                {
                    ref var engineComp = ref _enginePool.Value.Get(playerEntity);
                    ref var healthComp = ref _healthPool.Value.Get(playerEntity);
                    ref var buildProcess = ref _buildProcessPool.Value.Add(entity);

                    buildProcess.InitialDelay = Mathf.Clamp(engineComp.Delay - (engineComp.Delay * engineComp.BuildModifier), 0.1f, 2f);

                    float baseHealth = healthComp.MaxValue * engineComp.Health;
                    float finalHealth = baseHealth * (1f + engineComp.HealthModifier);

                    buildProcess.Health = finalHealth;
                }
            }
        }

        void StopBuild(int entity, Collider collider)
        {
            if(_buildProcessPool.Value.Has(entity)) 
                _buildProcessPool.Value.Del(entity);
        }
    }
}
