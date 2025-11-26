using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunActionSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<InActionState>, Exc<DeadComponent>> _filter = default;
        readonly EcsPoolInject<InActionState> _inActionPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var inactionComp = ref _inActionPool.Value.Get(entity);

                inactionComp.Duration -= Time.deltaTime;

                if (inactionComp.Duration <= 0)
                {
                    _inActionPool.Value.Del(entity);
                }
            }
        }
    }
}