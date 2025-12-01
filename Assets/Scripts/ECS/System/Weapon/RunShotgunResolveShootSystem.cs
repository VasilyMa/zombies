using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client
{
    sealed class RunShotgunResolveShootSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;

        readonly EcsFilterInject<Inc<RequestShootEvent, ShotgunComponent, WeaponComponent, RapidfireComponent, CountMissileComponent>, Exc<InFiretickState>> _filterWeapon = default;

        readonly EcsFilterInject<Inc<MissileComponent, BulletComponent, PoolComponent, TransformComponent>> _filterMissile = default;

        readonly EcsPoolInject<InFiretickState> _fireTickPool = default;
        readonly EcsPoolInject<RapidfireComponent> _rapidFirePool = default;
        readonly EcsPoolInject<PoolComponent> _pool = default;
        readonly EcsPoolInject<MissileSetupEvent> _setupPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<WeaponComponent> _weaponPool = default;
        readonly EcsPoolInject<SpreadComponent> _spreadPool = default;
        readonly EcsPoolInject<MissileComponent> _missilePool = default;
        readonly EcsPoolInject<RequestShootEvent> _requestPool = default;
        readonly EcsPoolInject<CountMissileComponent> _countPool = default;
        readonly EcsPoolInject<ParticleComponent> _particlePool = default;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterWeapon.Value)
            {
                ref var requestComp = ref _requestPool.Value.Get(entity);
                ref var weaponComp = ref _weaponPool.Value.Get(entity);
                ref var spreadComp = ref _spreadPool.Value.Get(entity);
                ref var countComp = ref _countPool.Value.Get(entity);

                // создаем setup событие
                ref var setupComp = ref _setupPool.Value.Add(entity);
                setupComp.MissileEntity = ListPool<int>.Get();

                // количество дробин в залпе
                int pellets = countComp.Count;

                // веерный угол (общий угол разброса)
                float angle = spreadComp.Angle;
                float angleStep = (pellets > 1) ? angle / (pellets - 1) : 0f;

                for (int s = 0; s < requestComp.ShotCount; s++)
                {
                    for (int i = 0; i < pellets; i++)
                    {
                        float currentAngle = -angle / 2f + angleStep * i;

                        int missileEntity = CreateOrTakeMissile(ref weaponComp, currentAngle);

                        setupComp.MissileEntity.Add(missileEntity);
                    }
                }

                ref var rapidFireComp = ref _rapidFirePool.Value.Get(entity);
                if (!_fireTickPool.Value.Has(entity)) _fireTickPool.Value.Add(entity).RemainingTime =
                    Mathf.Clamp(rapidFireComp.RapidfireSpeed - (rapidFireComp.RapidfireSpeed * rapidFireComp.Modifier), 0.01f, 10f);
            }
        }

        // СНАЧАЛА ПЫТАЕМСЯ ВЗЯТЬ ИЗ ПУЛА, ПОТОМ СОЗДАЕМ
        int CreateOrTakeMissile(ref WeaponComponent weaponComp, float angle)
        { 
            int missileEntity = -1;

            if (EntityPoolService.TryGet(weaponComp.MissilePrefab.name, out missileEntity))
            {  
                ref var transformComp = ref _transformPool.Value.Get(missileEntity);
                SetTransform(ref transformComp, ref weaponComp, angle);

            } 
            else
            {
                missileEntity = _world.Value.NewEntity();

                _pool.Value.Add(missileEntity).KeyName = weaponComp.MissilePrefab.name;

                var missileInstance = Object.Instantiate(weaponComp.MissilePrefab, weaponComp.FirePoint.position, Quaternion.identity);

                ref var missileComp = ref _missilePool.Value.Add(missileEntity);
                missileComp.KeyName = weaponComp.MissilePrefab.name;

                ref var transformComp = ref _transformPool.Value.Add(missileEntity);
                transformComp.Transform = missileInstance.transform;

                ref var paticleComp = ref _particlePool.Value.Add(missileEntity);
                paticleComp.Particles = missileInstance.GetComponentsInChildren<ParticleSystem>();

                SetTransform(ref transformComp, ref weaponComp, angle);
            }

            return missileEntity;
        }
         
        void SetTransform(ref TransformComponent transformComp, ref WeaponComponent weaponComp, float angle)
        {
            // базовая позиция и направление
            Transform firePoint = weaponComp.FirePoint;

            transformComp.Transform.position = firePoint.position;

            Vector3 baseDir = firePoint.forward;
            baseDir.y = 0f;
            baseDir.Normalize();

            // веерный поворот
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up);

            Vector3 finalDir = rot * baseDir;

            transformComp.Transform.rotation = Quaternion.LookRotation(finalDir, Vector3.up);
        }
    }
}
