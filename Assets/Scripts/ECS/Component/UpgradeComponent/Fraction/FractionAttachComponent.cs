using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct FractionAttachComponent : IRecyclable
    {
        public float DamageValue;
        public float Radius;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        { 

        }
    }
}
