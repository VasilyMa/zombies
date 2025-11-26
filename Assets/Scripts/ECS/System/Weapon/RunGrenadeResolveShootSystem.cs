using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client
{
    sealed class RunGrenadeResolveShootSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<RequestShootEvent, GrenadeComponent, WeaponComponent, RapidfireComponent, SpreadComponent>, Exc<InFiretickState>> _filterWeapon = default;
         
        readonly EcsPoolInject<InFiretickState> _fireTickPool = default;
        readonly EcsPoolInject<RapidfireComponent> _rapidFirePool = default;
        readonly EcsPoolInject<PoolComponent> _pool = default;
        readonly EcsPoolInject<MissileSetupEvent> _setupPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<WeaponComponent> _weaponPool = default;
        readonly EcsPoolInject<SpreadComponent> _spreadPool = default;
        readonly EcsPoolInject<MissileComponent> _missilePool = default;
        readonly EcsPoolInject<RequestShootEvent> _requestPool = default;
        readonly EcsPoolInject<ExplodeEffectComponent> _explodePool = default;
        readonly EcsPoolInject<GrenadeComponent> _grenadePool = default;
        readonly EcsPoolInject<BalisticComponent> _balisticPool = default;
        readonly EcsPoolInject<SpeedComponent> _speedPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterWeapon.Value)
            { 
                ref var requestComp = ref _requestPool.Value.Get(entity);
                ref var weaponComp = ref _weaponPool.Value.Get(entity);
                ref var spreadComp = ref _spreadPool.Value.Get(entity);
                ref var setupComp = ref _setupPool.Value.Add(entity);
                ref var grenadeComp = ref _grenadePool.Value.Get(entity);
                ref var speedComp = ref _speedPool.Value.Get(entity);

                setupComp.MissileEntity = ListPool<int>.Get();

                for (int i = 0; i < requestComp.ShotCount; i++)
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

                        var missileInstance = GameObject.Instantiate(
                            weaponComp.MissilePrefab,
                            Vector3.zero,
                            Quaternion.identity
                        );

                        ref var missileComp = ref _missilePool.Value.Add(missileEntity);
                        missileComp.KeyName = weaponComp.MissilePrefab.name;

                        ref var transformComp = ref _transformPool.Value.Add(missileEntity);
                        transformComp.Transform = missileInstance.transform;

                        _balisticPool.Value.Add(missileEntity); // добавл€ем Ѕаллистику
                        SetTransform(ref transformComp, ref weaponComp, ref spreadComp); 
                    } 
                    // --------------------------------------------------------
                    // 3) “”“ ћџ ¬—“ј¬Ћя≈ћ »Ќ»÷»јЋ»«ј÷»ё ЅјЋЋ»—“» »!
                    // --------------------------------------------------------
                    {
                        ref var transformComp = ref _transformPool.Value.Get(missileEntity);
                        ref var balisticComp = ref _balisticPool.Value.Get(missileEntity);

                        // StartPos Ч текуща€ позици€ после SetTransform()
                        balisticComp.StartPos = transformComp.Transform.position;

                        // EndPos Ч куда бросаем гранату
                        balisticComp.EndPos = requestComp.TargetPos;

                        // —читаем дистанцию
                        float distance = Vector3.Distance(balisticComp.StartPos, balisticComp.EndPos);

                        // скорость полЄта (добавь WeaponComponent.ProjectileSpeed если нужно)
                        float missileSpeed = speedComp.Value;

                        balisticComp.T = 0f;
                        balisticComp.TTarget = distance / missileSpeed;

                        // дуга траектории
                        float multi = Random.Range(2.5f, 5f);
                        balisticComp.P2 = balisticComp.StartPos + Vector3.up * multi;
                        balisticComp.P3 = balisticComp.EndPos + Vector3.up * multi;
                    }
                    // --------------------------------------------------------

                    // дл€ эффекта взрыва
                    ref var explodeComp = ref _explodePool.Value.Add(missileEntity);
                    explodeComp.ExplosionPrefab = grenadeComp.ExplosionPrefab;
                    explodeComp.RadiusArea = grenadeComp.Radius;

                    setupComp.MissileEntity.Add(missileEntity);

                    ref var rapidFireComp = ref _rapidFirePool.Value.Get(entity);
                    _fireTickPool.Value.Add(entity).RemainingTime = Mathf.Clamp(rapidFireComp.RapidfireSpeed - (rapidFireComp.RapidfireSpeed * rapidFireComp.Modifier), 0.01f, 10f);
                }
            }
        }

        void SetTransform(ref TransformComponent transformComp, ref WeaponComponent weaponComp, ref SpreadComponent spreadComp)
        {
            // позици€ и ротаци€
            transformComp.Transform.position = weaponComp.FirePoint.position;
            transformComp.Transform.rotation = weaponComp.FirePoint.rotation;

            // горизонтальный разброс
            float spreadAngle = spreadComp.Angle;
            float randomAngle = Random.Range(-spreadAngle, spreadAngle);

            Quaternion spreadRotation = Quaternion.Euler(0f, randomAngle, 0f);
            transformComp.Transform.rotation *= spreadRotation;
        }
    }
}
