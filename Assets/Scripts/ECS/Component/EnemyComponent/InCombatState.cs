using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct InCombatState : IRecyclable
    {
        public int TargetEntity;
        public float DistanceToAttack;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        {

        }
    }
}
