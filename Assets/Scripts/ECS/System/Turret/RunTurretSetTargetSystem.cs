using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunTurretSetTargetSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<SetTargetEvent, TurretComponent, AimComponent, TransformComponent, HealthComponent, TargetComponent>, Exc<RequestShootEvent>> _filter = default;
        readonly EcsPoolInject<TargetComponent> _targetPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<SetTargetEvent> _setTargetPool = default;
        readonly EcsPoolInject<RequestShootEvent> _requestShootPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var targetComp = ref _targetPool.Value.Get(entity);
                ref var transformComp = ref _transformPool.Value.Get(entity);
                ref var setTargetEvent = ref _setTargetPool.Value.Get(entity);

                float minDistance = float.MaxValue;

                Transform currentTarget = null;

                foreach (var target in setTargetEvent.Targets)
                {
                    float distance = Vector3.Distance(transformComp.Transform.position, target.transform.position);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        currentTarget = target.transform;
                    }
                } 

                if (currentTarget != null && _state.Value.TryGetEntity(currentTarget.name, out int targetEntity))
                {
                    targetComp.Entity = targetEntity;
                    targetComp.IsTarget = true;
                    _requestShootPool.Value.Add(entity);
                }
                else
                {
                    targetComp.IsTarget = false;
                }
            }
        }
    }
}