using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunAttackEnemyCastingSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<InAttackState, AnimateComponent>, Exc<DeadComponent, ResolveAttackEvent>> _filter = default;
        readonly EcsPoolInject<InAttackState> _inAttackPool = default;
        readonly EcsPoolInject<ResolveAttackEvent> _resolvePool = default;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var inAttackComp = ref _inAttackPool.Value.Get(entity);

                inAttackComp.Delay -= Time.deltaTime;

                if (inAttackComp.Delay <= 0)
                {
                    _resolvePool.Value.Add(entity);
                }
            }
        }
    }
}