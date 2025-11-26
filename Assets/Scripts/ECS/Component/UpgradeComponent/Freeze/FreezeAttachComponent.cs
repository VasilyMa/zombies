using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct FreezeAttachComponent : IRecyclable
    {
        public float Duration;
        public float Chance;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        { 

        }
    }
}
