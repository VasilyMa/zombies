using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunShootSetDamageSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<MissileSetupEvent, AttackComponent>> _filter = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;
        readonly EcsPoolInject<AttackComponent> _attackPool = default;
        readonly EcsPoolInject<MissileSetupEvent> _missilePool = default;


        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var missileComp = ref _missilePool.Value.Get(entity);
                ref var attackComp = ref _attackPool.Value.Get(entity);

                ref var damageComp = ref _damagePool.Value.Add(missileComp.MissileEntity);
                damageComp.Value = attackComp.Damage;
            }
        }
    }
}
