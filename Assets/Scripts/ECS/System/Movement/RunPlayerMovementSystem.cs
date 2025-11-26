using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client
{
    sealed class RunPlayerMovementSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;

        readonly EcsFilterInject<Inc<PlayerComponent, MovementComponent, InputMovementState, CharacterControllerComponent, AnimateComponent, BoundsComponent>, Exc<DeadComponent>> _filter = default;

        readonly EcsPoolInject<MovementComponent> _movePool = default;
        readonly EcsPoolInject<InputMovementState> _inputPool = default;
        readonly EcsPoolInject<CharacterControllerComponent> _characterPool = default;
        readonly EcsPoolInject<AnimateComponent> _animatePool = default;
        readonly EcsPoolInject<BoundsComponent> _boundsPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var characterComp = ref _characterPool.Value.Get(entity);
                ref var moveComp = ref _movePool.Value.Get(entity);
                ref var inputComp = ref _inputPool.Value.Get(entity);
                ref var animateComp = ref _animatePool.Value.Get(entity);
                ref var boundsComp = ref _boundsPool.Value.Get(entity);

                // ---------------------------------------------------
                // 1) Получаем направление движения
                // ---------------------------------------------------
                Vector3 moveDirection = new Vector3(
                    inputComp.MovementJoystick.Horizontal,
                    0f,
                    inputComp.MovementJoystick.Vertical
                );

                bool isMoving = moveDirection.sqrMagnitude > 0.0001f;

                // Нормализуем ТОЛЬКО если есть движение
                if (isMoving)
                    moveDirection.Normalize();
                else
                    moveDirection = Vector3.zero;
                // ---------------------------------------------------
                // 2) Двигаем CharacterController
                // ---------------------------------------------------
                Vector3 motion = moveDirection * moveComp.CurrentValue * Time.deltaTime;
                characterComp.Character.Move(motion);

                // ---------------------------------------------------
                // 2.1 Ограничиваем позицию игрока в пределах Bounds
                // ---------------------------------------------------
                Transform playerTransform = characterComp.Character.transform;

                // Текущая позиция после Move
                Vector3 pos = playerTransform.position;

                // Ограничение по XZ (поле боя — сверху)
                pos.x = Mathf.Clamp(pos.x, boundsComp.Min.x, boundsComp.Max.x);
                pos.z = Mathf.Clamp(pos.z, boundsComp.Min.y, boundsComp.Max.y);

                // НЕ трогаем Y (контроллер сам управляет высотой)
                playerTransform.position = pos;

                // ---------------------------------------------------
                // 3) Анимация через один Blend Tree
                // ---------------------------------------------------
                Animator animator = animateComp.Animator;

                animator.SetFloat("MoveX", moveDirection.x);
                animator.SetFloat("MoveZ", moveDirection.z);
            }
        }
    }
}
