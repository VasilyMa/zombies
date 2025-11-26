using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunSlowEffectSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<SlowEffectState, MovementComponent>> _filter = default;
        readonly EcsPoolInject<SlowEffectState> _slowEffectPool = default;
        readonly EcsPoolInject<MovementComponent> _movePool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var slowComp = ref _slowEffectPool.Value.Get(entity);
                slowComp.Delay -= Time.deltaTime;

                if (slowComp.Delay <= 0)
                {
                    ref var moveComp = ref _movePool.Value.Get(entity);
                    for (global::System.Int32 i = 0; i < slowComp.Stack; i++)
                    {
                        moveComp.RemoveModifier(slowComp.Effect);
                    }
                    _slowEffectPool.Value.Del(entity);
                }
            }
        }
    }
}