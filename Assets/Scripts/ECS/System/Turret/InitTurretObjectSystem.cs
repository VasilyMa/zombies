using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class InitTurretObjectSystem : IEcsInitSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsPoolInject<TurretComponent> _turretPool = default;
        readonly EcsPoolInject<BuildProcessState> _buildProcessPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<AimComponent> _aimPool = default;
        readonly EcsPoolInject<TargetComponent> _targetPool = default;
        readonly EcsPoolInject<RapidfireComponent> _rapidFirePool = default;
        readonly EcsPoolInject<SpreadComponent> _spreadPool = default;
        readonly EcsPoolInject<AttackComponent> _attackPool = default;
        readonly EcsPoolInject<SpeedComponent> _speedPool = default;
        readonly EcsPoolInject<EngineComponent> _enginePool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<DamageHandlerComponent> _damageHandlerPool = default;
        readonly EcsPoolInject<ConstructComponent> _constrcutPool = default;

        public void Init(IEcsSystems systems)
        {
            var turrets = GameObject.FindObjectsByType<TurretZone>(FindObjectsSortMode.None);

            var buildConfig = ConfigModule.GetConfig<BuildConfig>();

            var turretBase = buildConfig.TurretBases[0];

            foreach (var turret in turrets)
            {
                var turretEntity = _world.Value.NewEntity();

                turret.Entity = turretEntity;
                turret.TriggerEntered += StartBuild;
                turret.TriggerExited += StopBuild;

                ref var turretComp = ref _turretPool.Value.Add(turretEntity);
                turretComp.TargetProgress = 5;
                turretComp.TurretObject = turret.turretObject.transform;
                turretComp.FirePoint = turret.firePoint.transform;
                turretComp.MissilePrefab = turretBase.MissilePrefab;
                turretComp.RotationSpeed = turretBase.RotationSpeed;

                ref var transformComp = ref _transformPool.Value.Add(turretEntity);
                transformComp.Transform = turret.turretObject.transform;
                transformComp.Transform.gameObject.SetActive(false);

                ref var aimComp = ref _aimPool.Value.Add(turretEntity);
                aimComp.ViewDistance = turretBase.ViewDistance; //TODO SET DISTANCE VIEW

                ref var rapidFireComp = ref _rapidFirePool.Value.Add(turretEntity);
                rapidFireComp.RapidfireSpeed = turretBase.Rapidfire; //TODO SET RAPIDFIRE SPEED

                ref var spreadComp = ref _spreadPool.Value.Add(turretEntity);
                spreadComp.Angle = turretBase.Angle; //TODO SET SPREAD ANGLE

                ref var attackComp = ref _attackPool.Value.Add(turretEntity);
                attackComp.Damage = turretBase.Damage;

                ref var speedComp = ref _speedPool.Value.Add(turretEntity);
                speedComp.Value = turretBase.Speed;

                ref var targetComp = ref _targetPool.Value.Add(turretEntity);

                _state.Value.AddEntity(turretEntity.ToString(), turretEntity);

                turret.turretObject.name = turretEntity.ToString();

                _damageHandlerPool.Value.Add(turretEntity);
                _constrcutPool.Value.Add(turretEntity);
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
            if (_buildProcessPool.Value.Has(entity))
            {
                _buildProcessPool.Value.Del(entity);
            }
        }
    }
}
