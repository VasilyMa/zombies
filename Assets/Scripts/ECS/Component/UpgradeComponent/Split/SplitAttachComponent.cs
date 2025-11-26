using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct SplitAttachComponent : IRecyclable
    {
        public float Angle;
        public float DamageValue;
        public int Missiles;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        {

        }
    }
}
