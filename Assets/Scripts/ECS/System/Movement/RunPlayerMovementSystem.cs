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
        readonly EcsFilterInject<Inc<PlayerComponent, MovementComponent, InputMovementState, CharacterControllerComponent>> _filter = default;
        readonly EcsPoolInject<MovementComponent> _movePool = default;
        readonly EcsPoolInject<InputMovementState> _inputPool = default;
        readonly EcsPoolInject<CharacterControllerComponent> _characterPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var characterComp = ref _characterPool.Value.Get(entity);   
                ref var moveComp = ref _movePool.Value.Get(entity);
                ref var inputComp = ref _inputPool.Value.Get(entity);

                Vector3 moveDirection = new Vector3(inputComp.MovementJoystick.Horizontal, 0f, inputComp.MovementJoystick.Vertical);
                characterComp.Character.Move(moveDirection * moveComp.MoveSpeed * Time.deltaTime);
            }
        }
    }
}
