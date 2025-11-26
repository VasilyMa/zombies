using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    sealed class RunResolveMissileSplitSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;

        readonly EcsFilterInject<Inc<ResolveMissileEvent, SplitAttachComponent, TransformComponent, VelocityComponent, DamageComponent>> _filter = default;
        readonly EcsFilterInject<Inc<MissileComponent, BulletComponent, PoolComponent, TransformComponent>> _filterMissile = default;

        readonly EcsPoolInject<SplitAttachComponent> _attachPool = default;
        readonly EcsPoolInject<ResolveMissileEvent> _resolvePool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<HitEvent> _hitPool = default;
        readonly EcsPoolInject<PoolComponent> _pool = default;
        readonly EcsPoolInject<MissileComponent> _missilePool = default; 
        readonly EcsPoolInject<CompleteShootEvent> _completePool = default;
        readonly EcsPoolInject<BulletComponent> _bulletPool = default;
        readonly EcsPoolInject<SplitAttachComponent> _splitPool = default;
        readonly EcsPoolInject<VelocityComponent> _velocityPool = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;

        // pools for attach components
        readonly EcsPoolInject<FlameAttachComponent> _flameAttachPool = default;
        readonly EcsPoolInject<FreezeAttachComponent> _freezeAttachPool = default;
        readonly EcsPoolInject<FractionAttachComponent> _fractionAttachPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var srcEntity in _filter.Value)
            {
                ref var resolveComp = ref _resolvePool.Value.Get(srcEntity);
                ref var attachComp = ref _attachPool.Value.Get(srcEntity);
                ref var srcTransform = ref _transformPool.Value.Get(srcEntity);

                // если у источника есть MissileComponent Ч получим keyName дл€ поиска подход€щих пул-объектов
                string templateKey = null;

                if (_missilePool.Value.Has(srcEntity))
                {
                    ref var srcMissile = ref _missilePool.Value.Get(srcEntity);
                    templateKey = srcMissile.KeyName;
                }

                int missilesCount = Mathf.Max(attachComp.Missiles, 1);

                // обработка каждой попавшей сущности (Hit) Ч дл€ каждой цели делаем сплит
                foreach (var hitEntity in resolveComp.HitEntities)
                {
                    if (!_hitPool.Value.Has(hitEntity)) continue;
                    ref var hitComp = ref _hitPool.Value.Get(hitEntity);

                    // трансформ цели
                    if (!_transformPool.Value.Has(hitComp.TargetEntity)) continue;
                    ref var targetTransformComp = ref _transformPool.Value.Get(hitComp.TargetEntity);

                    // spawn position (пример Ч немного над целью)
                    Vector3 spawnPos = targetTransformComp.Transform.position + Vector3.up;

                    // direction = в спину цели
                    Vector3 backDirection = -targetTransformComp.Transform.forward;
                    backDirection.y = 0f;
                    backDirection.Normalize();

                    if (missilesCount <= 1)
                    {
                        // если всего один Ч просто "ребейсим" исходный мишнел/передаЄм как complete
                        int missileEntity = TakeOrCreateMissile(srcEntity, templateKey, srcTransform.Transform, spawnPos, backDirection); 
                        _completePool.Value.Add(missileEntity);
                        continue;
                    }

                    // расчЄт углового шага
                    float angle = attachComp.Angle;
                    float angleStep = (missilesCount > 1) ? angle / (missilesCount - 1) : 0f;

                    for (int i = 0; i < missilesCount; i++)
                    {
                        float currentAngle = -angle / 2f + i * angleStep;
                        Vector3 rotatedDirection = Quaternion.AngleAxis(currentAngle, Vector3.up) * backDirection;
                        rotatedDirection.y = 0f;
                        rotatedDirection.Normalize();

                        // дистанци€ дл€ расчЄта точечного targetPosition (по желанию)
                        float dst = Vector3.Distance(srcTransform.Transform.position, targetTransformComp.Transform.position);
                        Vector3 targetPosition = srcTransform.Transform.position + rotatedDirection * dst;

                        int missileEntity = TakeOrCreateMissile(srcEntity, templateKey, srcTransform.Transform, spawnPos, rotatedDirection);

                        ref var velocityComp = ref _velocityPool.Value.Get(srcEntity);
                        ref var damageComp = ref _damagePool.Value.Get(srcEntity);

                        ref var vlctyComp = ref _velocityPool.Value.Add(missileEntity);
                        vlctyComp.Speed = velocityComp.Speed;
                        ref var dmgComp = ref _damagePool.Value.Add(missileEntity);
                        dmgComp.Value = damageComp.Value * 0.1f;
                        // копируем attach эффекты (flame/freeze/fraction) с исходного на новый
                        CopyAttachIfHas(srcEntity, missileEntity);
                         
                        _completePool.Value.Add(missileEntity);
                    }
                }

                _splitPool.Value.Del(srcEntity);
            }
        }

        // ѕопытатьс€ вз€ть сущность из пула (filterMissile), подход€щую по key (если key == null Ч берем любую).
        // ≈сли нашли Ч удал€ем у неЄ PoolComponent (занимаем) и возвращаем id. »наче создаЄм новый.
        int TakeOrCreateMissile(int srcEntity, string templateKey, Transform srcTransform, Vector3 spawnPos, Vector3 direction)
        {
            // сначала попробуем найти в пуле подход€щую сущность
            foreach (var pooled in _filterMissile.Value)
            {
                // если templateKey задан Ч провер€ем совпадение
                if (_missilePool.Value.Has(pooled))
                {
                    ref var pooledMissile = ref _missilePool.Value.Get(pooled);
                    if (templateKey != null && pooledMissile.KeyName != templateKey)
                        continue;
                }

                // забираем этот пул-объект: удал€ем PoolComponent (маркируем как зан€та€)
                if (_pool.Value.Has(pooled))
                    _pool.Value.Del(pooled);

                // на pooled уже есть TransformComponent Ч установим позицию/ротацию
                if (_transformPool.Value.Has(pooled))
                {
                    ref var t = ref _transformPool.Value.Get(pooled);
                    SetTransform(ref t, spawnPos, direction);
                }

                return pooled;
            }

            // ничего подход€щего не найдено Ч создаЄм новую сущность и новый GameObject-клон исходного (fallback)
            int newEntity = _world.Value.NewEntity();

            ref var bulletComp = ref _bulletPool.Value.Add(newEntity);

            ref var newMissile = ref _missilePool.Value.Add(newEntity);
            newMissile.KeyName = templateKey;
             

            // создаЄм TransformComponent и инстансим игровой объект копию исходного transform's gameObject
            var instanceGO = Object.Instantiate(srcTransform.gameObject, spawnPos, Quaternion.LookRotation(direction, Vector3.up));
            ref var newTransform = ref _transformPool.Value.Add(newEntity);
            newTransform.Transform = instanceGO.transform;

            return newEntity;
        }

        //  опировать attach компоненты (если у src есть) в dst (добавить и присвоить)
        void CopyAttachIfHas(int src, int dst)
        {
            if (_flameAttachPool.Value.Has(src))
            {
                ref var srcComp = ref _flameAttachPool.Value.Get(src);
                ref var dstComp = ref _flameAttachPool.Value.Add(dst);
                dstComp = srcComp;
            }

            if (_freezeAttachPool.Value.Has(src))
            {
                ref var srcComp = ref _freezeAttachPool.Value.Get(src);
                ref var dstComp = ref _freezeAttachPool.Value.Add(dst);
                dstComp = srcComp;
            }

            if (_fractionAttachPool.Value.Has(src))
            {
                ref var srcComp = ref _fractionAttachPool.Value.Get(src);
                ref var dstComp = ref _fractionAttachPool.Value.Add(dst);
                dstComp = srcComp;
            }
        }

        void SetTransform(ref TransformComponent tc, Vector3 position, Vector3 direction)
        {
            tc.Transform.position = position;
            tc.Transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}
