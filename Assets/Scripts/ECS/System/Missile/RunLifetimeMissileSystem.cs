using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunLifetimeMissileSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<MissileMotionState, LifetimeComponent, TransformComponent>> _filter = default;
        readonly EcsPoolInject<LifetimeComponent> _lifePool = default;
        readonly EcsPoolInject<CleanUpEvent> _cleanUpPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var lifeComp = ref _lifePool.Value.Get(entity);

                lifeComp.RemainingTime -= Time.deltaTime;

                if (lifeComp.RemainingTime <= 0)
                {
                    ref var transformComp = ref _transformPool.Value.Get(entity);
                    transformComp.Transform.gameObject.SetActive(false);

                    _cleanUpPool.Value.Add(entity);
                }
            }
        }
    }
}