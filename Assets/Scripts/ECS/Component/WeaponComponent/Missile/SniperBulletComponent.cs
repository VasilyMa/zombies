using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct SniperBulletComponent : IRecyclable
    {
        public int CountCollision;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        {

        }
    }
}
