using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunChillEffectSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ChillEffectState, MovementComponent>> _filter = default;
        readonly EcsPoolInject<ChillEffectState> _chillStatePool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var chiilComp = ref _chillStatePool.Value.Get(entity);
                chiilComp.Delay -= Time.deltaTime;  

                if (chiilComp.Delay <= 0) _chillStatePool.Value.Del(entity); 
            }
        }
    }
}