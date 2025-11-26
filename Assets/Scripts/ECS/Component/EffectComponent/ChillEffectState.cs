using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct ChillEffectState : IRecyclable
    {
        public float Delay;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        {

        }
    }
}
