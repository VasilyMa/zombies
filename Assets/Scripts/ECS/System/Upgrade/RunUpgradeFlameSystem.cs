using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunUpgradeFlameSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<UpgradeFlameEvent, PlayerComponent, WeaponHolderComponent>> _filter = default;
        readonly EcsPoolInject<WeaponHolderComponent> _weaponHolderPool = default;
        readonly EcsPoolInject<FlameEffectComponent> _flamePool = default;
        readonly EcsPoolInject<UpgradeFlameEvent> _upgradeEventPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var upgradeEventComp = ref _upgradeEventPool.Value.Get(entity);
                ref var weaponHolderComp = ref _weaponHolderPool.Value.Get(entity);

                foreach (var weaponEntity in weaponHolderComp.WeaponEntities)
                {
                    if (!_flamePool.Value.Has(weaponEntity)) _flamePool.Value.Add(weaponEntity);

                    ref var flameComp = ref _flamePool.Value.Get(weaponEntity);
                    flameComp.Damage = upgradeEventComp.Damage;
                    flameComp.Duration = upgradeEventComp.Duration;
                    flameComp.Tick = upgradeEventComp.Tick;
                    flameComp.FlamePrefab = upgradeEventComp.FlamePrefab;
                }

            }
        }
    }
}