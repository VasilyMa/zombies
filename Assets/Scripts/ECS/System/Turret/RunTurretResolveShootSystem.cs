using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client
{
    sealed class RunTurretResolveShootSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
         
        readonly EcsFilterInject<Inc<RequestShootEvent, TurretComponent, HealthComponent, RapidfireComponent, SpreadComponent>, Exc<InFiretickState>> _filterTurrets = default; 
        readonly EcsFilterInject<Inc<MissileComponent, BulletComponent, PoolComponent, TransformComponent>> _filterMissile = default; 
        readonly EcsPoolInject<TurretComponent> _turretPool = default;
        readonly EcsPoolInject<InFiretickState> _fireTickPool = default;
        readonly EcsPoolInject<RapidfireComponent> _rapidFirePool = default;
        readonly EcsPoolInject<MissileSetupEvent> _setupPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<SpreadComponent> _spreadPool = default;
        readonly EcsPoolInject<MissileComponent> _missilePool = default;

        readonly EcsPoolInject<PoolComponent> _pool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterTurrets.Value)
            { 
                ref var turretComp = ref _turretPool.Value.Get(entity);
                ref var spreadComp = ref _spreadPool.Value.Get(entity);

                int missileEntity = -1;

                if (EntityPoolService.TryGet(turretComp.MissilePrefab.name, out missileEntity))
                { 
                    ref var transformComp = ref _transformPool.Value.Get(missileEntity);
                    SetTransform(ref transformComp, ref turretComp, ref spreadComp);
                }
                else
                {
                    missileEntity = _world.Value.NewEntity();
                    _pool.Value.Add(missileEntity).KeyName = turretComp.MissilePrefab.name;
                    var missileInstance = GameObject.Instantiate(turretComp.MissilePrefab, Vector3.zero, Quaternion.identity);

                    ref var missileComp = ref _missilePool.Value.Add(missileEntity);
                    missileComp.KeyName = turretComp.MissilePrefab.name;

                    ref var transformComp = ref _transformPool.Value.Add(missileEntity);
                    transformComp.Transform = missileInstance.transform;

                    SetTransform(ref transformComp, ref turretComp, ref spreadComp); 
                } 

                // --- Регистрируем событие настройки ---
                ref var setUpComp = ref _setupPool.Value.Add(entity);
                setUpComp.MissileEntity = ListPool<int>.Get();
                setUpComp.MissileEntity.Add(missileEntity);

                // --- Добавляем фаертик ---
                ref var rapidFireComp = ref _rapidFirePool.Value.Get(entity);
                _fireTickPool.Value.Add(entity).RemainingTime = Mathf.Clamp(rapidFireComp.RapidfireSpeed - (rapidFireComp.RapidfireSpeed * rapidFireComp.Modifier), 0.01f, 10f);
            }
        }

        void SetTransform(ref TransformComponent transformComp, ref TurretComponent turretComp, ref SpreadComponent spreadComp)
        {
            // --- Устанавливаем позицию и ротацию ---
            transformComp.Transform.position = turretComp.FirePoint.position;
            transformComp.Transform.rotation = turretComp.FirePoint.rotation;

            // --- Применяем горизонтальный разброс ---
            float spreadAngle = spreadComp.Angle;
            float randomAngle = Random.Range(-spreadAngle, spreadAngle);

            // Поворот вокруг оси Y (горизонтальный разброс)
            Quaternion spreadRotation = Quaternion.Euler(0f, randomAngle, 0f);
            transformComp.Transform.rotation *= spreadRotation;
        }
    }
}
