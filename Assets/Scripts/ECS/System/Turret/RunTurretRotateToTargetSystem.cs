using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunTurretRotateToTargetSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<TurretComponent, TransformComponent, TargetComponent, HealthComponent>> _filter = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<TargetComponent> _targetPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var targetComp = ref _targetPool.Value.Get(entity);

                if (!targetComp.IsTarget) continue;

                ref var turretTransformComp = ref _transformPool.Value.Get(entity);
                ref var targetTransformComp = ref _transformPool.Value.Get(targetComp.Entity);
                ref var turretComp = ref _world.Value.GetPool<TurretComponent>().Get(entity);

                Transform turret = turretComp.TurretObject;
                Transform target = targetTransformComp.Transform;

                Vector3 dir = target.position - turret.position;
                 
                dir.y = 0f;

                if (dir.sqrMagnitude < 0.001f) continue;

                Quaternion targetRotation = Quaternion.LookRotation(dir);

                // --- Плавный поворот ---
                turret.rotation = Quaternion.RotateTowards(
                    turret.rotation,
                    targetRotation,
                    turretComp.RotationSpeed * Time.deltaTime
                );
            }
        }
    }
}