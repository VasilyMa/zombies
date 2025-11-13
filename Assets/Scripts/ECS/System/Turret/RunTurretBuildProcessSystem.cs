using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunTurretBuildProcessSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<BuildProcessState, TurretComponent>, Exc<HealthComponent>> _filter = default;
        readonly EcsPoolInject<BuildProcessState> _buildProcessPool = default;
        readonly EcsPoolInject<TurretComponent> _turretPool = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;
        readonly EcsPoolInject<CompleteBuildEvent> _completeBuildPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var buildProcessComp = ref _buildProcessPool.Value.Get(entity);

                buildProcessComp.Delay -= Time.deltaTime;

                if (buildProcessComp.Delay < 0)
                {
                    if (_state.Value.TryGetEntity("player", out int playerEntity))
                    {
                        ref var playerComp = ref _playerPool.Value.Get(playerEntity);

                        if (playerComp.TrySpendMoney(1))
                        {
                            ref var turretComp = ref _turretPool.Value.Get(entity);
                            turretComp.CurrentProgress += 1;

                            if (turretComp.CurrentProgress >= turretComp.TargetProgress)
                            {
                                _completeBuildPool.Value.Add(entity);
                            }
                        }
                    }

                    buildProcessComp.Delay = buildProcessComp.InitialDelay;
                }
            }
        }
    }
}