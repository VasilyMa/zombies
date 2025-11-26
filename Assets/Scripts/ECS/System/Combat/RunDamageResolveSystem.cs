using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunDamageResolveSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<DamageHandlerComponent, HealthComponent>, Exc<DeadComponent>> _filter = default;
        readonly EcsPoolInject<DamageHandlerComponent> _damageHandlerPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var damageHandlerComp = ref _damageHandlerPool.Value.Get(entity);
                ref var healthComp = ref _healthPool.Value.Get(entity);

                healthComp.Sub(damageHandlerComp.DamageValue);

                damageHandlerComp.DamageValue = 0f;
            }
        }
    }
}