using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunShootSetLifeSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<MissileSetupEvent, DistanceComponent>, Exc<GrenadeComponent>> _filter = default;
        readonly EcsPoolInject<DistanceComponent> _distancePool = default;
        readonly EcsPoolInject<SpeedComponent> _speedPool = default;
        readonly EcsPoolInject<MissileSetupEvent> _missilePool = default;
        readonly EcsPoolInject<LifetimeComponent> _lifePool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var missileComp = ref _missilePool.Value.Get(entity); 
                ref var distanceComp = ref _distancePool.Value.Get(entity);
                ref var speedComp = ref _speedPool.Value.Get(entity);
                 
                float life = distanceComp.Value / speedComp.Value;

                foreach (var missileEntity in missileComp.MissileEntity)
                {
                    _lifePool.Value.Add(missileEntity).RemainingTime = life;
                }
            }
        }
    }
}