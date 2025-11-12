using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunRequestShootSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<EnemyComponent>, Exc<DeadComponent, PoolComponent>> _enemyFilter = default;
        readonly EcsFilterInject<Inc<WeaponComponent>, Exc<InFiretickState, RequestShootEvent>> _weaponFilter = default;
        readonly EcsPoolInject<RequestShootEvent> _shootPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var enemyEntity in _enemyFilter.Value)
            {
                foreach (var weaponEntity in _weaponFilter.Value)
                {
                    _shootPool.Value.Add(weaponEntity);
                }
            }
        }
    }
}
