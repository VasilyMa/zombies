using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunUpgradeSplitShotSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<UpgradeSplitShotEvent, PlayerComponent, WeaponHolderComponent>> _filter = default;
        readonly EcsPoolInject<WeaponHolderComponent> _weaponHolderPool = default;
        readonly EcsPoolInject<SplitEffectComponent> _splitPool = default;
        readonly EcsPoolInject<UpgradeSplitShotEvent> _upgradeEventPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var upgradeEventComp = ref _upgradeEventPool.Value.Get(entity);
                ref var weaponHolderComp = ref _weaponHolderPool.Value.Get(entity);

                foreach (var weaponEntity in weaponHolderComp.WeaponEntities)
                {
                    if (!_splitPool.Value.Has(weaponEntity)) _splitPool.Value.Add(weaponEntity);

                    ref var splitComp = ref _splitPool.Value.Get(weaponEntity);
                    splitComp.AdditionalMissile = upgradeEventComp.AdditionalMissile;
                    splitComp.DamageValue = upgradeEventComp.DamageValue;
                    splitComp.Angle = upgradeEventComp.Angle;
                }

            }
        }
    }
}