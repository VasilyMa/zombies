using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client
{
    sealed class RunMotionMissileSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<MissileComponent, MissileMotionState, TransformComponent, VelocityComponent>> _filter = default;
        readonly EcsPoolInject<VelocityComponent> _velocityPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var velocity = ref _velocityPool.Value.Get(entity);
                ref var transformComp = ref _transformPool.Value.Get(entity);

                transformComp.Transform.position += transformComp.Transform.forward * velocity.Speed * Time.deltaTime;
            }
        }
    }
}
