using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct InFiretickState : IRecyclable
    {
        public float RemainingTime;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        {

        }
    }
}
