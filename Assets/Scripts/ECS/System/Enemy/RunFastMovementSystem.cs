using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client
{
    sealed class RunFastMovementSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;

        readonly EcsFilterInject<Inc<EnemyComponent, MovementComponent, TransformComponent, FastComponent>, Exc<DeadComponent, InActionState, InCombatState, LockState>> _enemyFilter = default;

        readonly EcsPoolInject<MovementComponent> _movePool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<FastComponent> _fastPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var enemy in _enemyFilter.Value)
            {
                ref var fast = ref _fastPool.Value.Get(enemy);
                ref var move = ref _movePool.Value.Get(enemy);
                ref var trComp = ref _transformPool.Value.Get(enemy);
                Transform tr = trComp.Transform;

                // --- базовое направление движения (не меняется системой поворота) ---
                Vector3 baseDir = fast.BaseDirection;
                baseDir.y = 0f;
                if (baseDir.sqrMagnitude < 0.0001f)
                {
                    // защита: если не задано, то берём текущий forward (единожды)
                    baseDir = tr.forward;
                    baseDir.y = 0f;
                    if (baseDir.sqrMagnitude < 0.0001f) baseDir = Vector3.forward;
                    baseDir.Normalize();
                    fast.BaseDirection = baseDir;
                }

                // --- прямое движение вдоль baseDir ---
                Vector3 forwardMove = baseDir * move.CurrentValue;

                // --- зигзаг: вычисляем правый вектор относительно baseDir (перпендикуляр на XZ) ---
                Vector3 right = Vector3.Cross(Vector3.up, baseDir).normalized; // это "право" для baseDir

                fast.ZigzagTime += Time.deltaTime * fast.ZigzagSpeed;
                float offset = Mathf.Sin(fast.ZigzagTime) * fast.ZigzagAmplitude;
                Vector3 sideMove = right * offset;

                // --- итоговая скорость (логическое движение) ---
                Vector3 velocity = forwardMove + sideMove;

                // --- перемещение (по позиции) ---
                Vector3 newPos = tr.position + velocity * Time.deltaTime;

                // --- ограничение по зоне ---
                newPos.x = Mathf.Clamp(newPos.x, fast.Min.x, fast.Max.x);
                newPos.z = Mathf.Clamp(newPos.z, fast.Min.y, fast.Max.y);

                tr.position = newPos;

                // --- ВИЗУАЛЬНЫЙ поворот (можно поворачивать модель/child, но мы поворачиваем transform визуально) ---
                // Поворачиваемся в сторону фактической скорости (velocity), но НЕ используем этот поворот
                // для вычисления baseDir. BaseDir остаётся прежним.
                Vector3 lookDir = velocity;
                lookDir.y = 0f;
                if (lookDir.sqrMagnitude > 0.0001f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(lookDir);
                    tr.rotation = Quaternion.Slerp(tr.rotation, targetRot, fast.RotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
