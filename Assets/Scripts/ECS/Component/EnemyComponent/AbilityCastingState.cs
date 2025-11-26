using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct AbilityCastingState : IRecyclable
    {
        public float Delay;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        { 
        }
    }
}
