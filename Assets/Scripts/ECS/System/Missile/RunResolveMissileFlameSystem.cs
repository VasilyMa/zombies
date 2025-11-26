using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunResolveMissileFlameSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ResolveMissileEvent, FlameAttachComponent>> _fitler = default;
        readonly EcsPoolInject<FlameAttachComponent> _attachPool = default;
        readonly EcsPoolInject<ResolveMissileEvent> _resolvePool = default;
        readonly EcsPoolInject<ThrowFlameEvent> _throwPool= default; 

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _fitler.Value)
            {
                ref var resolveComp = ref _resolvePool.Value.Get(entity);
                ref var attachComp = ref _attachPool.Value.Get(
                    entity);

                foreach (var hitEntity in resolveComp.HitEntities)
                {
                    ref var throwFlameComp = ref _throwPool.Value.Add(hitEntity);
                    throwFlameComp.Damage = attachComp.DamageValue;
                    throwFlameComp.Duration = attachComp.Duration;
                    throwFlameComp.Tick = attachComp.Tick;
                    throwFlameComp.FlamePrefab = attachComp.FlamePrefab;
                }

                _attachPool.Value.Del(entity);
            }
        }
    }
}