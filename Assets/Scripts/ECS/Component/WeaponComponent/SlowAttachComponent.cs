using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct SlowAttachComponent : IRecyclable
    {
        public float SlowEffect;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        {

        }
    }
}
