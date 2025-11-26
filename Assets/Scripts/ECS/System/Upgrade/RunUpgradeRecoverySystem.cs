using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunUpgradeRecoverySystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<UpgradeRecoveryEvent, PlayerComponent, RecoveryComponent>> _filter = default;
        readonly EcsPoolInject<RecoveryComponent> _recoveryPool = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;
        readonly EcsPoolInject<UpgradeRecoveryEvent> _upgradePool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var playerComp = ref _playerPool.Value.Get(entity);
                ref var recoveryComp = ref _recoveryPool.Value.Get(entity);
                ref var upgradeComp = ref _upgradePool.Value.Get(entity);

                recoveryComp.Value += upgradeComp.BonusValue;
            }
        }
    }
}