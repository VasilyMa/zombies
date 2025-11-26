using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client
{
    sealed class RunShootSetSpeedSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<MissileSetupEvent, SpeedComponent>> _filter = default;
        readonly EcsPoolInject<MissileSetupEvent> _missilePool = default;
        readonly EcsPoolInject<SpeedComponent> _speedPool = default;
        readonly EcsPoolInject<VelocityComponent> _velocityPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var missileComp = ref _missilePool.Value.Get(entity);
                ref var speedComp = ref _speedPool.Value.Get(entity);

                foreach (var missileEntity in missileComp.MissileEntity)
                {
                    _velocityPool.Value.Add(missileEntity).Speed = speedComp.Value;
                } 
            }
        }
    }
}
