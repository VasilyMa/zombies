using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct HitEvent : IDisposable 
    {
        public int TargetEntity;

        public void Dispose(EcsWorld world, BattleState state, int entity)
        {
            world.DelEntity(entity);
        }
    }
}
