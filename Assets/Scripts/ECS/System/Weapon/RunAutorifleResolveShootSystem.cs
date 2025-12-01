using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client
{
    sealed class RunAutorifleResolveShootSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<RequestShootEvent, AutoRifleComponent, WeaponComponent, RapidfireComponent, SpreadComponent>, Exc<InFiretickState>> _filterWeapon = default;

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
        readonly EcsPoolInject<SlowAttachComponent> _slowEffectPool = default;
        readonly EcsPoolInject<ParticleComponent> _particlePool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterWeapon.Value)
            { 
                ref var requestComp = ref _requestPool.Value.Get(entity);
                ref var weaponComp = ref _weaponPool.Value.Get(entity);
                ref var spreadComp = ref _spreadPool.Value.Get(entity);
                ref var setupComp = ref _setupPool.Value.Add(entity);

                setupComp.MissileEntity = ListPool<int>.Get();

                for (global::System.Int32 i = 0; i < requestComp.ShotCount; i++)
                {
                    int missileEntity = -1;

                    if (EntityPoolService.TryGet(weaponComp.MissilePrefab.name, out missileEntity))
                    {
                        ref var transformComp = ref _transformPool.Value.Get(missileEntity);
                        SetTransform(ref transformComp, ref weaponComp, ref spreadComp); 
                    }
                    else
                    {
                        missileEntity = _world.Value.NewEntity();
                        _pool.Value.Add(missileEntity).KeyName = weaponComp.MissilePrefab.name;
                        var missileInstance = GameObject.Instantiate(weaponComp.MissilePrefab, weaponComp.FirePoint.position, Quaternion.identity);

                        ref var missileComp = ref _missilePool.Value.Add(missileEntity);
                        missileComp.KeyName = weaponComp.MissilePrefab.name;
                        ref var transformComp = ref _transformPool.Value.Add(missileEntity);
                        transformComp.Transform = missileInstance.transform;
                        ref var paticleComp = ref _particlePool.Value.Add(missileEntity);
                        paticleComp.Particles = missileInstance.GetComponentsInChildren<ParticleSystem>();


                        SetTransform(ref transformComp, ref weaponComp, ref spreadComp);
                    }  

                    // --- Регистрируем событие настройки ---
                    _slowEffectPool.Value.Add(missileEntity).SlowEffect = 0.05f;
                    setupComp.MissileEntity.Add(missileEntity);

                    ref var rapidFireComp = ref _rapidFirePool.Value.Get(entity);
                    if (!_fireTickPool.Value.Has(entity)) _fireTickPool.Value.Add(entity).RemainingTime = Mathf.Clamp(rapidFireComp.RapidfireSpeed - (rapidFireComp.RapidfireSpeed * rapidFireComp.Modifier), 0.01f, 10f);
                }
            }
        }

        void SetTransform(ref TransformComponent transformComp, ref WeaponComponent weaponComp, ref SpreadComponent spreadComp)
        {
            // --- Устанавливаем позицию и ротацию ---
            transformComp.Transform.position = weaponComp.FirePoint.position;
            transformComp.Transform.rotation = weaponComp.FirePoint.rotation;

            // --- Применяем горизонтальный разброс ---
            float spreadAngle = spreadComp.Angle; // например, 5 градусов
            float randomAngle = Random.Range(-spreadAngle, spreadAngle);

            // Поворот вокруг оси Y — горизонтально
            Quaternion spreadRotation = Quaternion.Euler(0f, randomAngle, 0f);
            transformComp.Transform.rotation *= spreadRotation;
        }
    }
}