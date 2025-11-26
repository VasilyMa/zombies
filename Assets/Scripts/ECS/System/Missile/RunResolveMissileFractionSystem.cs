using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunResolveMissileFractionSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ResolveMissileEvent, FractionAttachComponent>> _fitler = default;
        readonly EcsPoolInject<FractionAttachComponent> _attachPool = default;
        readonly EcsPoolInject<ResolveMissileEvent> _resolvePool = default; 
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<HitEvent> _hitPool = default;
        readonly EcsPoolInject<ThrowDamageEvent> _throwDamagePool = default;

        int layerMask = LayerMask.GetMask("Enemy");
        Collider[] colliders = new Collider[20];

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _fitler.Value)
            {
                ref var resolveComp = ref _resolvePool.Value.Get(entity);
                ref var attachComp = ref _attachPool.Value.Get(
                    entity);

                foreach (var hitEntity in resolveComp.HitEntities)
                {
                    ref var hitComp = ref _hitPool.Value.Get(hitEntity);

                    if (_transformPool.Value.Has(hitComp.TargetEntity))
                    {
                        ref var tranformComp = ref _transformPool.Value.Get(hitComp.TargetEntity);

                        var hits = Physics.OverlapSphereNonAlloc(tranformComp.Transform.position, attachComp.Radius, colliders, layerMask);

                        foreach (var collider in colliders)
                        {
                            if (collider == null) break;

                            if (_state.Value.TryGetEntity(collider.name, out int targetEntity))
                            { 
                                if (!_throwDamagePool.Value.Has(targetEntity)) _throwDamagePool.Value.Add(targetEntity);

                                ref var takeDamageComp = ref _throwDamagePool.Value.Get(targetEntity);
                                takeDamageComp.Damage += attachComp.DamageValue;
                            } 
                        }
                    } 
                }

                _attachPool.Value.Del(entity);
            }
        }
    }
}