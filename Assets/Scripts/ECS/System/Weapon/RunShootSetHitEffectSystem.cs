using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunShootSetHitEffectSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<MissileSetupEvent, HitEffectComponent>> _filter = default;
        readonly EcsPoolInject<HitEffectComponent> _hitPool = default; 
        readonly EcsPoolInject<HitAttachComponent> _attachPool = default; 
        readonly EcsPoolInject<MissileSetupEvent> _missilePool = default;


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var missileComp = ref _missilePool.Value.Get(entity);
                ref var hit = ref _hitPool.Value.Get(entity);

                foreach (var missileEntity in missileComp.MissileEntity)
                {
                    ref var hitComp = ref _attachPool.Value.Add(missileEntity);
                    hitComp.Prefab = hit.Prefab;
                }
            }
        }
    }
}