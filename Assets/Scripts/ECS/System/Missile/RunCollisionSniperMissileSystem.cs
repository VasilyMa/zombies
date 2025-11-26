using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunCollisionSniperMissileSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<MissileCollisionEvent, TransformComponent, MissileMotionState, SniperBulletComponent>, Exc<CleanUpEvent>> _filter = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<MissileCollisionEvent> _missileCollisionPool = default;
        readonly EcsPoolInject<ResolveMissileEvent> _resolveMissilePool = default; 
        readonly EcsPoolInject<HitEvent> _hitPool = default; 
        readonly EcsPoolInject<SniperBulletComponent> _sniperPool = default;
        readonly EcsPoolInject<CleanUpEvent> _cleanUpPool = default;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var sniperComp = ref _sniperPool.Value.Get(entity);
                ref var collisionComp = ref _missileCollisionPool.Value.Get(entity);
                ref var resolveComp = ref _resolveMissilePool.Value.Add(entity);
                ref var transformComp = ref _transformPool.Value.Get(entity);
                 
                int capacity = collisionComp.TargetEntities.Length;

                resolveComp.HitEntities = ListPool<int>.Get();

                for (global::System.Int32 i = 0; i < collisionComp.TargetEntities.Length; i++)
                {
                    var hitEntity = _world.Value.NewEntity();

                    var targetEntity = collisionComp.TargetEntities[i];

                    ref var hitComp = ref _hitPool.Value.Add(hitEntity);
                    hitComp.TargetEntity = targetEntity;

                    resolveComp.HitEntities.Add(hitEntity);
                }

                sniperComp.CountCollision--;

                if (sniperComp.CountCollision <= 0)
                { 
                    transformComp.Transform.gameObject.SetActive(false);

                    _cleanUpPool.Value.Add(entity);
                } 
            }
        }
    }
}