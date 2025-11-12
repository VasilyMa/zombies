using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunResolveMissileDamageSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ResolveMissileEvent, DamageComponent>> _fitler = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;
        readonly EcsPoolInject<ResolveMissileEvent> _resolvePool = default;
        readonly EcsPoolInject<ThrowDamageEvent> _throwPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _fitler.Value)
            {
                ref var resolveComp = ref _resolvePool.Value.Get(entity);
                ref var damageComp = ref _damagePool.Value.Get(entity);

                foreach (var hitEntity in resolveComp.HitEntities)
                {
                    ref var throwDamageComp = ref _throwPool.Value.Add(hitEntity);
                    throwDamageComp.Damage = damageComp.Value;
                }
            }
        }
    }
}
