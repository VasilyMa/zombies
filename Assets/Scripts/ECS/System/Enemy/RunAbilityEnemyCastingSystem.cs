using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunAbilityEnemyCastingSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<AbilityCastingState, AnimateComponent>, Exc<DeadComponent, ResolveAbilityEvent>> _filter = default;
        readonly EcsPoolInject<AbilityCastingState> _castingStatePool = default;
        readonly EcsPoolInject<ResolveAbilityEvent> _resolvePool = default;
        readonly EcsPoolInject<AnimateComponent> _animatorPool = default;
        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var castingComp = ref _castingStatePool.Value.Get(entity);
                ref var animateComp = ref _animatorPool.Value.Get(entity);
                castingComp.Delay -= Time.deltaTime;
                 
                if (castingComp.Delay <= 0)
                { 
                    _resolvePool.Value.Add(entity);
                }
            }
        }
    }
}