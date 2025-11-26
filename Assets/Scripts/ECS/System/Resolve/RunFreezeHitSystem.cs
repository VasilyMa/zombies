using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunFreezeHitSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ResolveHitEvent, HitEvent, ThrowFreezeEvent>> _filter = default;
        readonly EcsPoolInject<HitEvent> _hitPool = default; 
        readonly EcsPoolInject<ThrowFreezeEvent> _throwFreezePool = default;
        readonly EcsPoolInject<ChillEffectState> _takeChillPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var hitComp = ref _hitPool.Value.Get(entity);
                ref var throwFreezeComp = ref _throwFreezePool.Value.Get(entity);

                if (!_takeChillPool.Value.Has(hitComp.TargetEntity)) _takeChillPool.Value.Add(hitComp.TargetEntity);

                ref var takeChillComp = ref _takeChillPool.Value.Get(hitComp.TargetEntity);
                takeChillComp.Delay = throwFreezeComp.Duration;
            }
        }
    }
}