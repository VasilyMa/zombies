using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunBurnEffectSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<BurnEffectState, HealthComponent, DamageHandlerComponent>, Exc<DeadComponent>> _filter = default;
        readonly EcsPoolInject<BurnEffectState> _burnStatePool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default; 
        readonly EcsPoolInject<DamageHandlerComponent> _damagePool = default; 
        readonly EcsPoolInject<CleanUpEvent> _cleanPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var burnComp = ref _burnStatePool.Value.Get(entity);

                burnComp.Delay -= Time.deltaTime;
                burnComp.EffectTransform.position = burnComp.TargetTransform.position + Vector3.up;
                
                if (burnComp.Delay <= 0)
                {
                    ref var healthComp = ref _healthPool.Value.Get(entity);

                    burnComp.Duration--;
                    burnComp.Delay = burnComp.Tick;

                    var damageValue = burnComp.Damage * healthComp.MaxValue;

                    _damagePool.Value.Get(entity).DamageValue += damageValue;

                    if (burnComp.Duration <= 0)
                    {
                        GameObjectPoolService.Release(burnComp.EffectName, burnComp.EffectTransform.gameObject);
                        _burnStatePool.Value.Del(entity); 
                    }
                }
            }
        }
    }
}