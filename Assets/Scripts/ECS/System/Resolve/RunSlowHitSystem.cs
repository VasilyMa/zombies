using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunSlowHitSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ResolveHitEvent, HitEvent, ThrowSlowEvent>> _filter = default;
        readonly EcsPoolInject<HitEvent> _hitPool = default;
        readonly EcsPoolInject<ThrowSlowEvent> _throwSlowPool = default;
        readonly EcsPoolInject<SlowEffectState> _takeSlowPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var hitComp = ref _hitPool.Value.Get(entity);
                ref var throwSlowComp = ref _throwSlowPool.Value.Get(entity);

                if (!_takeSlowPool.Value.Has(hitComp.TargetEntity)) _takeSlowPool.Value.Add(hitComp.TargetEntity);

                ref var takeSlowComp = ref _takeSlowPool.Value.Get(hitComp.TargetEntity);
                takeSlowComp.Delay = throwSlowComp.SlowValue;
                takeSlowComp.Stack++;
            }
        }
    }
}