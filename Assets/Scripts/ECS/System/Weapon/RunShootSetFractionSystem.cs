using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunShootSetFractionSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<MissileSetupEvent, FractionAttachComponent, AttackComponent>> _filter = default;
        readonly EcsPoolInject<MissileSetupEvent> _missilePool = default;
        readonly EcsPoolInject<FractionEffectComponent> _fractionPool = default;
        readonly EcsPoolInject<FractionAttachComponent> _attachPool = default;
        readonly EcsPoolInject<AttackComponent> _attackPool = default;


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var missileComp = ref _missilePool.Value.Get(entity);
                ref var fractionComp = ref _fractionPool.Value.Get(entity);
                ref var attackComp = ref _attackPool.Value.Get(entity);

                foreach (var missileEntity in missileComp.MissileEntity)
                { 
                    ref var attachComp = ref _attachPool.Value.Add(missileEntity);
                    attachComp.Radius = fractionComp.Radius;
                    attachComp.DamageValue = attackComp.Damage * fractionComp.DamageValue;
                }
            }
        }
    }
}