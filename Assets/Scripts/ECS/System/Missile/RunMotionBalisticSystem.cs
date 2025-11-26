using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client
{
    sealed class RunMotionBalisticSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;

        readonly EcsFilterInject<Inc<MissileComponent, MissileMotionState, BalisticComponent, TransformComponent, VelocityComponent, ExplodeEffectComponent>> _filter;
        readonly EcsPoolInject<BalisticComponent> _balisticPool;
        readonly EcsPoolInject<TransformComponent> _transformPool;
        readonly EcsPoolInject<VelocityComponent> _velocityPool;
        readonly EcsPoolInject<MissileMotionState> _motionPool; 
        readonly EcsPoolInject<MissileCollisionEvent> _collisionPool = default;
        readonly EcsPoolInject<ExplodeEffectComponent> _explodePool = default; 

        int layerMask = LayerMask.GetMask("Enemy");

        Collider[] colliders = new Collider[20];
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var bal = ref _balisticPool.Value.Get(entity);
                ref var tr = ref _transformPool.Value.Get(entity);
                ref var vel = ref _velocityPool.Value.Get(entity);
                ref var motionState = ref _motionPool.Value.Get(entity); 

                bal.T += Time.deltaTime;

                if (bal.T > bal.TTarget)
                {
                    ref var explodeComp = ref _explodePool.Value.Get(entity);

                    int hits = Physics.OverlapSphereNonAlloc(tr.Transform.position, explodeComp.RadiusArea, colliders, layerMask);

                    ref var collisionComp = ref _collisionPool.Value.Add(entity);
                    collisionComp.TargetEntities = new int[hits];

                    for (global::System.Int32 i = 0; i < hits; i++)
                    {
                        if (_state.Value.TryGetEntity(colliders[i].name, out int targetEntity))
                        {
                            collisionComp.TargetEntities[i] = targetEntity;
                        }
                    } 
                    continue;
                }

                float tNorm = bal.T / bal.TTarget; 
                Vector3 pos = Ballistic(
                    bal.StartPos, bal.P2, bal.P3, bal.EndPos, tNorm
                );
                 
                tr.Transform.position = pos; 
                tr.Transform.LookAt(pos);
            }
        }
         
        private Vector3 Ballistic(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
        {
            Vector3 p12 = Vector3.Lerp(p1, p2, t);
            Vector3 p23 = Vector3.Lerp(p2, p3, t);
            Vector3 p34 = Vector3.Lerp(p3, p4, t);

            Vector3 p123 = Vector3.Lerp(p12, p23, t);
            Vector3 p234 = Vector3.Lerp(p23, p34, t);

            return Vector3.Lerp(p123, p234, t);
        }
    }
}
