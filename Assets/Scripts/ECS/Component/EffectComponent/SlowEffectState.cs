using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct SlowEffectState : IRecyclable
    {
        public float Effect;
        public float Delay;
        public int Stack;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        {

        }
    }
}
