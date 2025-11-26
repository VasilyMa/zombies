using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunResolveMissileNearSystem : IEcsRunSystem 
    { 
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ResolveMissileEvent, DamageComponent, NearEffectComponent>> _fitler = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;
        readonly EcsPoolInject<NearEffectComponent> _nearPool = default;
        readonly EcsPoolInject<ResolveMissileEvent> _resolvePool = default;
        readonly EcsPoolInject<ThrowDamageEvent> _throwPool = default;
        readonly EcsPoolInject<HitEvent> _hitPool = default;
        readonly EcsPoolInject<TransformComponent> _transfomPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _fitler.Value)
            {
                ref var resolveComp = ref _resolvePool.Value.Get(entity);
                ref var damageComp = ref _damagePool.Value.Get(entity);
                ref var nearComp = ref _nearPool.Value.Get(entity);

                if (_state.Value.TryGetEntity("player", out int playerEntity))
                {
                    ref var playerTransformComp = ref _transfomPool.Value.Get(playerEntity);

                    foreach (var hitEntity in resolveComp.HitEntities)
                    {
                        ref var hitComp = ref _hitPool.Value.Get(hitEntity);
                        ref var targetTransformComp = ref _transfomPool.Value.Get(hitComp.TargetEntity);

                        if (Vector3.Distance(playerTransformComp.Transform.position, targetTransformComp.Transform.position) < 5f)
                        { 
                            ref var throwDamageComp = ref _throwPool.Value.Add(hitEntity);
                            throwDamageComp.Damage = damageComp.Value + (damageComp.Value * nearComp.Bonus);
                        }
                        else
                        { 
                            ref var throwDamageComp = ref _throwPool.Value.Add(hitEntity);
                            throwDamageComp.Damage = damageComp.Value;
                        }
                    }
                }

                _nearPool.Value.Del(entity);
            }
        }
    }
}