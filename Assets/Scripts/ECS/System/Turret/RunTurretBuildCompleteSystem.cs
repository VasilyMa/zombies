using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunTurretBuildCompleteSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<CompleteBuildEvent, TurretComponent, BuildProcessState, TransformComponent, AttackComponent, RapidfireComponent>, Exc<HealthComponent>> _filter = default;
        readonly EcsPoolInject<TurretComponent> _turretPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<BuildProcessState> _buildProcessPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<AttackComponent> _attackPool = default;
        readonly EcsPoolInject<HasteComponent> _hastePool = default;
        readonly EcsPoolInject<PowerComponent> _powerPool = default;
        readonly EcsPoolInject<RapidfireComponent> _rapidPool = default; 

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var buildProcessComp = ref _buildProcessPool.Value.Get(entity);

                if (_state.Value.TryGetEntity("player", out int playerEntity))
                {
                    ref var hasteComp = ref _hastePool.Value.Get(playerEntity);
                    ref var powerComp = ref _powerPool.Value.Get(playerEntity);
                    ref var rapidFireComp = ref _rapidPool.Value.Get(entity);
                    rapidFireComp.Modifier = hasteComp.MaxValue;
                    ref var attackComp = ref _attackPool.Value.Get(entity); 
                    attackComp.Modifier = powerComp.MaxValue;
                }

                ref var turretComp = ref _turretPool.Value.Get(entity);
                turretComp.CurrentProgress = 0;
                ref var healthComp = ref _healthPool.Value.Add(entity);
                healthComp.Init(buildProcessComp.Health);
                ref var transformComp = ref _transformPool.Value.Get(entity);
                transformComp.Transform.gameObject.SetActive(true);

                _buildProcessPool.Value.Del(entity);
            }
        }
    }
}