using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunUpgradeStatSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<UpgradeStatEvent, PlayerComponent>> _filter = default;
        readonly EcsFilterInject<Inc<PlayerComponent, HealthComponent>> _filterHealth = default;
        readonly EcsFilterInject<Inc<PlayerComponent, MovementComponent>> _filterMove = default;
        readonly EcsFilterInject<Inc<PlayerComponent, HasteComponent>> _filterHaste = default;
        readonly EcsFilterInject<Inc<PlayerComponent, EngineComponent>> _filterEngine = default;
        readonly EcsFilterInject<Inc<PlayerComponent, PowerComponent, WeaponHolderComponent>> _filterPower = default; 
        readonly EcsFilterInject<Inc<TurretComponent, AttackComponent>> _filterTurret = default; 
        readonly EcsPoolInject<HealthComponent> _healtPool = default; 
        readonly EcsPoolInject<PowerComponent> _powerPool = default; 
        readonly EcsPoolInject<UpgradeStatEvent> _statPool = default;
        readonly EcsPoolInject<WeaponHolderComponent> _weaponHolderPool = default;
        readonly EcsPoolInject<AttackComponent> _attackPool = default;
        readonly EcsPoolInject<MovementComponent> _movePool = default;
        readonly EcsPoolInject<RapidfireComponent> _rapidPool = default;
        readonly EcsPoolInject<HasteComponent> _hastePool = default;
        readonly EcsPoolInject<EngineComponent> _enginePool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var statComp = ref _statPool.Value.Get(entity);

                foreach (var e in _filterEngine.Value)
                {
                    ref var engineComp = ref _enginePool.Value.Get(entity);
                    engineComp.BuildModifier += statComp.BuildProcessBonus;
                    engineComp.HealthModifier += statComp.BuildHealthBonus;
                }

                foreach (var e in _filterHealth.Value)
                { 
                    ref var healthComp = ref _healtPool.Value.Get(e);
                    healthComp.AddModifier(statComp.PlayerHealthBonus);
                }

                foreach (var e in _filterPower.Value)
                { 
                    ref var powerComp = ref _powerPool.Value.Get(e);
                    powerComp.AddModifier(statComp.PlayerDamageBonus);

                    ref var weaponHolderComp = ref _weaponHolderPool.Value.Get(e);

                    foreach (var we in weaponHolderComp.WeaponEntities)
                    {
                        if (_attackPool.Value.Has(we))
                        {
                            ref var attackComp = ref _attackPool.Value.Get(we);
                            attackComp.Modifier = powerComp.MaxValue;
                        }
                    }

                    foreach (var te in _filterTurret.Value)
                    {
                        ref var attackComp = ref _attackPool.Value.Get(te);
                        attackComp.Modifier = powerComp.MaxValue;
                    }
                }

                foreach (var he in _filterHaste.Value)
                {
                    ref var hasteComp = ref _hastePool.Value.Get(he);
                    hasteComp.AddModifier(statComp.PlayerAttackSpeedBonus);

                    ref var weaponHolderComp = ref _weaponHolderPool.Value.Get(he);

                    foreach (var we in weaponHolderComp.WeaponEntities)
                    {
                        if (_rapidPool.Value.Has(we))
                        {
                            ref var rapidComp = ref _rapidPool.Value.Get(we);
                            rapidComp.Modifier = hasteComp.MaxValue;
                        }
                    }


                    foreach (var te in _filterTurret.Value)
                    {
                        if (_rapidPool.Value.Has(te))
                        {
                            ref var rapidComp = ref _rapidPool.Value.Get(te);
                            rapidComp.Modifier = hasteComp.MaxValue;
                        }
                    }
                }

                foreach (var e in _filterMove.Value)
                {
                    ref var moveComp = ref _movePool.Value.Get(e);
                    moveComp.AddModifier(statComp.PlayerMoveSpeedBonus);
                }
            }
        }
    }
}