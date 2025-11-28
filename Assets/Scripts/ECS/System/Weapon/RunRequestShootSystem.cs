using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client
{
    sealed class RunRequestShootSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;

        readonly EcsFilterInject<Inc<EnemyComponent>, Exc<DeadComponent, LockState>> _enemyFilter = default;
        readonly EcsFilterInject<Inc<WeaponComponent>, Exc<InFiretickState, RequestShootEvent, LockState>> _weaponFilter = default;

        readonly EcsPoolInject<RequestShootEvent> _shootPool = default;
        readonly EcsPoolInject<WeaponComponent> _weaponPool = default;

        int layerMask = LayerMask.GetMask("Enemy");

        public void Run(IEcsSystems systems)
        {
            foreach (var enemyEntity in _enemyFilter.Value)
            {
                foreach (var weaponEntity in _weaponFilter.Value)
                {
                    ref var weaponComp = ref _weaponPool.Value.Get(weaponEntity);
                    Transform fp = weaponComp.FirePoint;
                     
                    if (Physics.Raycast(fp.position, fp.forward, out RaycastHit hit, 100f, layerMask))
                    { 
                        ref var requestComp = ref _shootPool.Value.Add(weaponEntity);

                        requestComp.ShotCount = 1;
                        requestComp.TargetPos = hit.point; 
                    }
                }
            }
        }
    }
}
