using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client
{
    sealed class RunUpgradeFractionShotSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<UpgradeFractionShotEvent, PlayerComponent, WeaponHolderComponent>> _filter = default;
        readonly EcsPoolInject<WeaponHolderComponent> _weaponHolderPool = default;
        readonly EcsPoolInject<FractionEffectComponent> _fractionPool = default;
        readonly EcsPoolInject<UpgradeFractionShotEvent> _upgradeEventPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var upgradeEventComp = ref _upgradeEventPool.Value.Get(entity);
                ref var weaponHolderComp = ref _weaponHolderPool.Value.Get(entity);

                foreach (var weaponEntity in weaponHolderComp.WeaponEntities)
                {
                    if (!_fractionPool.Value.Has(weaponEntity)) _fractionPool.Value.Add(weaponEntity);

                    ref var fractionComp = ref _fractionPool.Value.Get(weaponEntity);
                    fractionComp.DamageValue = upgradeEventComp.DamageValue;
                    fractionComp.Radius = upgradeEventComp.Radius; 
                } 
            }
        }
    }
}