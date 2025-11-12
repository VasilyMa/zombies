using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunTakeDamageSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<TakeDamageEvent, HealthComponent>, Exc<DeadComponent>> _filter = default;
        readonly EcsPoolInject<TakeDamageEvent> _takeDamagePool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                ref var healthComp = ref _healthPool.Value.Get(entity);

                healthComp.CurrentValue -= takeDamageComp.DamageValue;
            }
        }
    }
}