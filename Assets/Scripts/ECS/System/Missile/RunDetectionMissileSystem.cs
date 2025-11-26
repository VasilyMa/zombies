using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunDetectionMissileSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<MissileComponent, MissileMotionState, TransformComponent, VelocityComponent>, Exc<BalisticComponent>> _filter = default;
        readonly EcsPoolInject<MissileCollisionEvent> _collisionPool = default;
        readonly EcsPoolInject<TransformComponent> _tranformPool = default;
        readonly EcsPoolInject<MissileMotionState> _motionStatePool = default;

        RaycastHit[] raycastHits = new RaycastHit[1];

        int layerMask = LayerMask.GetMask("Enemy", "Obstacle", "Environment");

        public void Run (IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var transformComp = ref _tranformPool.Value.Get(entity);
                ref var motionStateComp = ref _motionStatePool.Value.Get(entity);

                motionStateComp.DetectionDelay -= Time.deltaTime;

                if (motionStateComp.DetectionDelay < 0)
                { 
                    Ray ray = new Ray(transformComp.Transform.position, transformComp.Transform.forward);

                    var hits = Physics.RaycastNonAlloc(ray, raycastHits, 0.2f, layerMask);

                    if (hits > 0)
                    {
                        ref var collisionComp = ref _collisionPool.Value.Add(entity);
                        collisionComp.TargetEntities = new int[hits];

                        for (global::System.Int32 i = 0; i < raycastHits.Length; i++)
                        {
                            if (_state.Value.TryGetEntity(raycastHits[i].transform.name, out int targetEntity))
                            { 
                                collisionComp.TargetEntities[i] = targetEntity;
                            }
                        }
                    }
                    else
                    {
                        motionStateComp.DetectionDelay = 0.005f;
                    }
                }
            }
        }
    }
}
