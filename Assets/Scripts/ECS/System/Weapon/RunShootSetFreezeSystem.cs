using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunShootSetFreezeSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<MissileSetupEvent, FreezeEffectComponent>> _filter = default;
        readonly EcsPoolInject<MissileSetupEvent> _missilePool = default;
        readonly EcsPoolInject<FreezeEffectComponent> _freezePool = default;
        readonly EcsPoolInject<FreezeAttachComponent> _attachPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var missileComp = ref _missilePool.Value.Get(entity);
                ref var freezeComp = ref _freezePool.Value.Get(entity);

                foreach (var missileEntity in missileComp.MissileEntity)
                { 
                    ref var attachComp = ref _attachPool.Value.Add(missileEntity);
                    attachComp.Duration = freezeComp.Duration;
                    attachComp.Chance = freezeComp.Chance;
                }
            }
        }
    }
}