using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client
{
    sealed class RunAfterDieExplosionSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<DieEvent, ExplosionComponent, TransformComponent>> _filter = default;
        readonly EcsPoolInject<ExplosionComponent> _explosionPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<ResolveHitEvent> _resolvePool = default;
        readonly EcsPoolInject<HitEvent> _hitPool = default;
        readonly EcsPoolInject<ThrowDamageEvent> _throwPool = default;
        readonly EcsPoolInject<VFXComponent> _vfxPool = default;
        readonly EcsPoolInject<LifetimeComponent> _lifePool = default;

        int layerMask = LayerMask.GetMask("Enemy", "Player", "Construction");

        Collider[] colliders = new Collider[20];
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var tr = ref _transformPool.Value.Get(entity);

                ref var explosionComp = ref _explosionPool.Value.Get(entity);

                int hits = Physics.OverlapSphereNonAlloc(tr.Transform.position, explosionComp.Radius, colliders, layerMask);

                for (global::System.Int32 i = 0; i < hits; i++)
                {
                    if (_state.Value.TryGetEntity(colliders[i].name, out int targetEntity))
                    {
                        var resolveEntity = _world.Value.NewEntity();

                        _resolvePool.Value.Add(resolveEntity);
                        _throwPool.Value.Add(resolveEntity).Damage = explosionComp.Damage;
                        _hitPool.Value.Add(resolveEntity).TargetEntity = targetEntity;
                    }
                }

                var explodeEntity = -1;

                if (EntityPoolService.TryGet(explosionComp.ExplosionPrefab.name, out explodeEntity))
                {
                    ref var tc = ref _transformPool.Value.Get(explodeEntity);
                    tc.Transform.position = tr.Transform.position;
                    tc.Transform.gameObject.SetActive(true);
                }
                else
                {
                    explodeEntity = _world.Value.NewEntity();

                    ref var tc = ref _transformPool.Value.Add(explodeEntity);
                    ref var vfx = ref _vfxPool.Value.Add(explodeEntity);

                    var instance = GameObject.Instantiate(explosionComp.ExplosionPrefab, tr.Transform.position, Quaternion.identity);
                    tc.Transform = instance.transform;
                    tc.Transform.gameObject.SetActive(true);
                }

                _lifePool.Value.Add(explodeEntity).RemainingTime = 2f;
            }
        }
    }
}