using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunFlameHitSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ResolveHitEvent, HitEvent, ThrowFlameEvent>> _filter = default;
        readonly EcsPoolInject<HitEvent> _hitPool = default;
        readonly EcsPoolInject<ThrowFlameEvent> _throwFlamePool = default;
        readonly EcsPoolInject<BurnEffectState> _burnPool = default; 
        readonly EcsPoolInject<TransformComponent> _transformPool = default; 

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var hitComp = ref _hitPool.Value.Get(entity);
                ref var throwFlameComp = ref _throwFlamePool.Value.Get(entity);

                if (!_burnPool.Value.Has(hitComp.TargetEntity))
                { 
                    ref var burnComp = ref _burnPool.Value.Add(hitComp.TargetEntity);
                    burnComp.Tick = throwFlameComp.Tick;
                    burnComp.Duration = throwFlameComp.Duration;
                    burnComp.Damage = throwFlameComp.Damage;
                    burnComp.EffectName = throwFlameComp.FlamePrefab.name; 
                     
                    ref var targetTransformComp = ref _transformPool.Value.Get(hitComp.TargetEntity);

                    GameObject instanceFlame = null;

                    if (GameObjectPoolService.TryGet(throwFlameComp.FlamePrefab.name, out instanceFlame))
                    {

                    }
                    else
                    {
                        instanceFlame = GameObject.Instantiate(throwFlameComp.FlamePrefab, targetTransformComp.Transform.position, Quaternion.identity);  
                    } 

                    burnComp.EffectTransform = instanceFlame.transform;  
                    burnComp.TargetTransform = targetTransformComp.Transform;
                }
                else
                { 
                    ref var takeBurnComp = ref _burnPool.Value.Get(hitComp.TargetEntity);
                    takeBurnComp.Tick = throwFlameComp.Tick;
                    takeBurnComp.Duration = throwFlameComp.Duration;
                    takeBurnComp.Damage = throwFlameComp.Damage;
                }
            }
        }
    }
}