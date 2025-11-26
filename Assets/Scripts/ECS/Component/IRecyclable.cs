using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    interface IRecyclable
    {
        void Cleanup(EcsWorld world, BattleState state, int entity);
    }
}
