using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunRequestSetDoubleSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<RequestShootEvent, DoubleShotEffectComponent>> _filter = default;
        readonly EcsPoolInject<DoubleShotEffectComponent> _doublePool = default;
        readonly EcsPoolInject<RequestShootEvent> _requsetPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var doubleComp = ref _doublePool.Value.Get(entity);
                
                if (doubleComp.Chance > Random.value)
                {
                    ref var requestComp = ref _requsetPool.Value.Get(entity);
                    requestComp.ShotCount++;
                }
            }
        }
    }
}