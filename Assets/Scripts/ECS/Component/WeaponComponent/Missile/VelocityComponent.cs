using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct VelocityComponent : IRecyclable
    {
        public float Speed;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        { 
        }
    }
}
