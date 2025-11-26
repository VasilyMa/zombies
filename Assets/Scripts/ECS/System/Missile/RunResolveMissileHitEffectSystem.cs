using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunResolveMissileHitEffectSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ResolveMissileEvent, TransformComponent, HitAttachComponent>> _filter = default;
        readonly EcsPoolInject<HitAttachComponent> _hitPool = default;
        readonly EcsPoolInject<VFXComponent> _vfxPool;
        readonly EcsPoolInject<LifetimeComponent> _lifePool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _filter.Value)
            {
                ref var hitComp = ref _hitPool.Value.Get(e);
                ref var transformComp = ref _transformPool.Value.Get(e);

                var vfxEntity = -1;

                if (EntityPoolService.TryGet(hitComp.Prefab.name, out vfxEntity))
                {
                    ref var tc = ref _transformPool.Value.Get(vfxEntity);
                    tc.Transform.position = transformComp.Transform.position;
                    tc.Transform.gameObject.SetActive(true);
                }
                else
                {
                    vfxEntity = _world.Value.NewEntity();

                    ref var tc = ref _transformPool.Value.Add(vfxEntity);
                    ref var vfx = ref _vfxPool.Value.Add(vfxEntity);

                    var instance = GameObject.Instantiate(hitComp.Prefab, transformComp.Transform.position, Quaternion.identity);
                    tc.Transform = instance.transform;
                    tc.Transform.gameObject.SetActive(true);
                }

                _lifePool.Value.Add(vfxEntity).RemainingTime = 2f;
            }
        }
    }
}