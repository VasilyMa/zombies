using Leopotam.EcsLite;
using Leopotam.EcsLite.Di; 
using Statement; 

namespace Client 
{
    sealed class RunResolveMissileSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ResolveMissileEvent>> _filter = default;
        readonly EcsPoolInject<ResolveMissileEvent> _resolvePool = default;
        readonly EcsPoolInject<ResolveHitEvent> _resolveHitPool = default; 

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var resolveComp = ref _resolvePool.Value.Get(entity);

                foreach (var hitEntity in resolveComp.HitEntities)
                {
                    _resolveHitPool.Value.Add(hitEntity);
                }

                ListPool<int>.Release(resolveComp.HitEntities);
            }
        }
    }
}
