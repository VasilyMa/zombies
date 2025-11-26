using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
 
namespace Client 
{
    sealed class RunDamageHitSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ResolveHitEvent, HitEvent, ThrowDamageEvent>> _filter = default;
        readonly EcsPoolInject<HitEvent> _hitPool = default;
        readonly EcsPoolInject<ThrowDamageEvent> _throwDamagePool = default; 
        readonly EcsPoolInject<DamageHandlerComponent> _damageHandlerPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var hitComp = ref _hitPool.Value.Get(entity);
                ref var throwDmageComp = ref _throwDamagePool.Value.Get(entity);

                if (!_damageHandlerPool.Value.Has(hitComp.TargetEntity)) _damageHandlerPool.Value.Add(hitComp.TargetEntity);

                ref var damageComp = ref _damageHandlerPool.Value.Get(hitComp.TargetEntity);

                damageComp.DamageValue += throwDmageComp.Damage;
            }
        }
    }
}
