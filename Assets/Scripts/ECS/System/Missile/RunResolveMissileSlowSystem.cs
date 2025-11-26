using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunResolveMissileSlowSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ResolveMissileEvent, SlowAttachComponent>> _fitler = default;
        readonly EcsPoolInject<SlowAttachComponent> _slowPool = default;
        readonly EcsPoolInject<ResolveMissileEvent> _resolvePool = default;
        readonly EcsPoolInject<ThrowSlowEvent> _throwPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _fitler.Value)
            {
                ref var resolveComp = ref _resolvePool.Value.Get(entity);
                ref var slowComp = ref _slowPool.Value.Get(entity);
                
                foreach (var hitEntity in resolveComp.HitEntities)
                {
                    ref var throwFreezeComp = ref _throwPool.Value.Add(hitEntity);
                    throwFreezeComp.SlowValue = slowComp.SlowEffect;
                }

                _slowPool.Value.Del(entity);
            }
        }
    }
}