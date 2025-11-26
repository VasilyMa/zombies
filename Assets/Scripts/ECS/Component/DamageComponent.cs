using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct DamageComponent : IRecyclable
    {
        public float Value;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        { 

        }
    }
}
