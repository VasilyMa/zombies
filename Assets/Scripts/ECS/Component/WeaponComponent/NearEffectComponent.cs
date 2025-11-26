using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct NearEffectComponent : IRecyclable 
    {
        public float Bonus;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        {

        }
    }
}
