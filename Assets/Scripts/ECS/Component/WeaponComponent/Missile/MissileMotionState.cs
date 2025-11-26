using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct MissileMotionState : IRecyclable
    {
        public float DetectionDelay;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        {

        }
    }
}
