using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunTurretBuildCompleteSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<CompleteBuildEvent, TurretComponent, BuildProcessState, TransformComponent>, Exc<HealthComponent>> _filter = default;
        readonly EcsPoolInject<TurretComponent> _turretPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<BuildProcessState> _buildProcessPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var turretComp = ref _turretPool.Value.Get(entity);
                turretComp.CurrentProgress = 0;
                ref var healthComp = ref _healthPool.Value.Add(entity);
                healthComp.Init(10);
                ref var transformComp = ref _transformPool.Value.Get(entity);
                transformComp.Transform.gameObject.SetActive(true);

                _buildProcessPool.Value.Del(entity);
            }
        }
    }
}