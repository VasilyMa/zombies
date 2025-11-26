using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client
{
    sealed class RunShootSetSplitSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<MissileSetupEvent, SplitEffectComponent, AttackComponent>> _filter = default;
        readonly EcsPoolInject<SplitAttachComponent> _attachPool = default;
        readonly EcsPoolInject<SplitEffectComponent> _splitPool = default;
        readonly EcsPoolInject<MissileSetupEvent> _missilePool = default;
        readonly EcsPoolInject<AttackComponent> _attackPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var missileComp = ref _missilePool.Value.Get(entity);
                ref var splitComp = ref _splitPool.Value.Get(entity);
                ref var attackComp = ref _attackPool.Value.Get(entity);

                foreach (var missileEntity in missileComp.MissileEntity)
                {
                    ref var attachComp = ref _attachPool.Value.Add(missileEntity);
                    attachComp.Angle = splitComp.Angle;
                    attachComp.Missiles = splitComp.AdditionalMissile;
                    attachComp.DamageValue = attackComp.Damage * splitComp.DamageValue;
                }
            }
        }
    }
}