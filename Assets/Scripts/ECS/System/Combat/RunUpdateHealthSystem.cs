using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunUpdateHealthSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<HealthComponent, DamageHandlerComponent>, Exc<DieEvent, DeadComponent>> _filter = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<DieEvent> _diePool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var healtComp = ref _healthPool.Value.Get(entity);

                if (healtComp.CurrentValue <= 0)
                {
                    _diePool.Value.Add(entity);
                }
            }
        }
    }
}