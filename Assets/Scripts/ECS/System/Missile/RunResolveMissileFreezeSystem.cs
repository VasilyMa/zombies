using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client
{
    sealed class RunResolveMissileFreezeSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ResolveMissileEvent, FreezeAttachComponent>> _fitler = default;
        readonly EcsPoolInject<FreezeAttachComponent> _freezePool = default;
        readonly EcsPoolInject<ResolveMissileEvent> _resolvePool = default;
        readonly EcsPoolInject<ThrowFreezeEvent> _throwPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _fitler.Value)
            {
                ref var resolveComp = ref _resolvePool.Value.Get(entity);
                ref var freezeComp = ref _freezePool.Value.Get(entity);

                if (Random.value <= freezeComp.Chance)
                {
                    foreach (var hitEntity in resolveComp.HitEntities)
                    {
                        ref var throwFreezeComp = ref _throwPool.Value.Add(hitEntity);
                        throwFreezeComp.Duration = freezeComp.Duration;
                    }
                }

                _freezePool.Value.Del(entity);
            }
        }
    }
}