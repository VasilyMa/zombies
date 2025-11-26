using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct InputMovementState : IDisposable
    {
        public MovementJoystick MovementJoystick;
         
        public void Dispose(EcsWorld world, BattleState state, int entity)
        {
            if (state.TryGetEntity("player", out int playerEntity))
            {
                if (world.GetPool<CharacterControllerComponent>().Has(playerEntity))
                {
                    ref var characterComp = ref world.GetPool<CharacterControllerComponent>().Get(playerEntity);
                    characterComp.Character.Move(UnityEngine.Vector3.zero);
                }
                if (world.GetPool<AnimateComponent>().Has(playerEntity))
                {
                    ref var animateComp = ref world.GetPool<AnimateComponent>().Get(playerEntity);
                    animateComp.Animator.SetFloat("MoveX", 0f);
                    animateComp.Animator.SetFloat("MoveZ", 0f);
                }
            }

            world.GetPool<InputMovementState>().Del(entity);
        }
    }
}
