using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunShootSetFlameSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<MissileSetupEvent, FlameEffectComponent>> _filter = default;
        readonly EcsPoolInject<MissileSetupEvent> _missilePool = default;
        readonly EcsPoolInject<FlameEffectComponent> _flamePool = default;
        readonly EcsPoolInject<FlameAttachComponent> _attachPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            { 
                ref var missileComp = ref _missilePool.Value.Get(entity);
                ref var flameComp = ref _flamePool.Value.Get(entity);

                foreach (var missileEntity in missileComp.MissileEntity)
                { 
                    ref var attachComp = ref _attachPool.Value.Add(missileEntity);
                    attachComp.Tick = flameComp.Tick;
                    attachComp.DamageValue = flameComp.Damage;
                    attachComp.Duration = flameComp.Duration;
                    attachComp.FlamePrefab = flameComp.FlamePrefab;
                } 
            }
        }
    }
}