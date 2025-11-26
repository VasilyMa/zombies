using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct InActionState : IRecyclable
    {
        public float Duration;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        { 
        }
    }
}
