using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunUpgradeDoubleShotSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<UpgradeDoubleShotEvent, PlayerComponent, WeaponHolderComponent>> _filter = default;
        readonly EcsPoolInject<WeaponHolderComponent> _weaponHolderPool = default;
        readonly EcsPoolInject<DoubleShotEffectComponent> _doublePool = default;
        readonly EcsPoolInject<UpgradeDoubleShotEvent> _upgradeEventPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var upgradeEventComp = ref _upgradeEventPool.Value.Get(entity);
                ref var weaponHolderComp = ref _weaponHolderPool.Value.Get(entity);

                foreach (var weaponEntity in weaponHolderComp.WeaponEntities)
                {
                    if (!_doublePool.Value.Has(weaponEntity)) _doublePool.Value.Add(weaponEntity);

                    ref var doubleComp = ref _doublePool.Value.Get(weaponEntity);
                    doubleComp.Chance = upgradeEventComp.Chance;
                }

            }
        }
    }
}