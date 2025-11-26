using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunUpgradeFreezeSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<UpgradeFreezeShotEvent, PlayerComponent, WeaponHolderComponent>> _filter = default;
        readonly EcsPoolInject<UpgradeFreezeShotEvent> _upgradePool = default;
        readonly EcsPoolInject<FreezeEffectComponent> _freezePool = default;
        readonly EcsPoolInject<WeaponHolderComponent> _weaponHolderPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            { 
                ref var upgradeComp = ref _upgradePool.Value.Get(entity);
                ref var weaponHolderComp = ref _weaponHolderPool.Value.Get(entity);

                foreach (var weaponEntity in weaponHolderComp.WeaponEntities)
                {
                    if (!_freezePool.Value.Has(weaponEntity)) _freezePool.Value.Add(weaponEntity);

                    ref var freezeComp = ref _freezePool.Value.Get(weaponEntity);
                    freezeComp.Duration = upgradeComp.Duration; 
                    freezeComp.Chance = upgradeComp.Chance;
                }
            }
        }
    }
}