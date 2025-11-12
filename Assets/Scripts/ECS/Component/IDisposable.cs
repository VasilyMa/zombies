using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    interface IDisposable 
    {
        void Dispose(EcsWorld world, BattleState state, int entity);
    }
}
