using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using System.Collections.Generic;

namespace Client 
{
    sealed class RunCollisionMissileSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<MissileCollisionEvent, TransformComponent, MissileMotionState>> _filter = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<MissileCollisionEvent> _missileCollisionPool = default;
        readonly EcsPoolInject<ResolveMissileEvent> _resolveMissilePool = default;
        readonly EcsPoolInject<ReturnToPoolEvent> _returnToPool = default;
        readonly EcsPoolInject<DieEvent> _diePool = default;
        readonly EcsPoolInject<HitEvent> _hitPool = default;
        readonly EcsPoolInject<MissileMotionState> _motionPool = default;
        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var collisionComp = ref _missileCollisionPool.Value.Get(entity);
                ref var resolveComp = ref _resolveMissilePool.Value.Add(entity);
                ref var transformComp = ref _transformPool.Value.Get(entity);

                int capacity = collisionComp.TargetEntities.Length;

                resolveComp.HitEntities = new List<int>(); 

                for (global::System.Int32 i = 0; i < collisionComp.TargetEntities.Length; i++)
                {
                    var hitEntity = _world.Value.NewEntity();

                    var targetEntity = collisionComp.TargetEntities[i];

                    ref var hitComp = ref _hitPool.Value.Add(hitEntity);
                    hitComp.TargetEntity = targetEntity;

                    resolveComp.HitEntities.Add(hitEntity);
                }

                transformComp.Transform.gameObject.SetActive(false);

                _diePool.Value.Add(entity);
                _returnToPool.Value.Add(entity);

                _motionPool.Value.Del(entity);
            }
        }
    }
}
