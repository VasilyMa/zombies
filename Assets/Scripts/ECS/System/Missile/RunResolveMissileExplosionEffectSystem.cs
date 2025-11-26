using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using Unity.VisualScripting;
using UnityEngine;

namespace Client 
{
    sealed class RunResolveMissileExplosionEffectSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ResolveMissileEvent, TransformComponent, ExplodeEffectComponent>> _filter = default;
        readonly EcsPoolInject<ExplodeEffectComponent> _explodePool = default;
        readonly EcsPoolInject<VFXComponent> _vfxPool;
        readonly EcsPoolInject<LifetimeComponent> _lifePool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var e in _filter.Value)
            {
                ref var explodeComp = ref _explodePool.Value.Get(e);
                ref var transformComp = ref _transformPool.Value.Get(e);

                var explodeEntity = -1;

                if (EntityPoolService.TryGet(explodeComp.ExplosionPrefab.name, out explodeEntity))
                {
                    ref var tc = ref _transformPool.Value.Get(explodeEntity);
                    tc.Transform.position = transformComp.Transform.position;
                    tc.Transform.gameObject.SetActive(true);
                }
                else
                {
                    explodeEntity = _world.Value.NewEntity();

                    ref var tc = ref _transformPool.Value.Add(explodeEntity);
                    ref var vfx = ref _vfxPool.Value.Add(explodeEntity);

                    var instance = GameObject.Instantiate(explodeComp.ExplosionPrefab, transformComp.Transform.position, Quaternion.identity);
                    tc.Transform = instance.transform;
                    tc.Transform.gameObject.SetActive(true);
                }

                _lifePool.Value.Add(explodeEntity).RemainingTime = 2f;
            }
        }
    }
}