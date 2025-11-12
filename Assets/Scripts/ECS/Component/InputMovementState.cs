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
            }

            world.GetPool<InputMovementState>().Del(entity);
        }
    }
}
