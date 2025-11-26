using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunAbilityEnemyResolveSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<AbilityComponent, ResolveAbilityEvent, AbilityCastingState>, Exc<DeadComponent>> _filter = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly EcsPoolInject<CooldownComponent> _cooldownPool = default;
        readonly EcsPoolInject<AbilityCastingState> _abilityState = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var abilityComp = ref _abilityPool.Value.Get(entity);
                abilityComp.AbilityBase.Resolve(_world.Value, _state.Value, entity);

                _cooldownPool.Value.Add(entity).RemainingTime = abilityComp.AbilityBase.CooldownTime;

                _abilityState.Value.Del(entity);
            }
        }
    }
}