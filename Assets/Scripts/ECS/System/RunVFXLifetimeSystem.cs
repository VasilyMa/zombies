using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunVFXLifetimeSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<LifetimeComponent, VFXComponent>> _filter = default;
        readonly EcsPoolInject<CleanUpEvent> _cleanPool = default;
        readonly EcsPoolInject<LifetimeComponent> _lifePool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var lifeComp = ref _lifePool.Value.Get(entity);
                lifeComp.RemainingTime -= Time.deltaTime;

                if (lifeComp.RemainingTime <= 0)
                {
                    _cleanPool.Value.Add(entity);
                }
            }
        }
    }
}