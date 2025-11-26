using Leopotam.EcsLite;
using Statement;
using UnityEngine;

namespace Client 
{
    struct FlameAttachComponent : IRecyclable
    {
        public float DamageValue;
        public float Tick;
        public float Duration;
        public GameObject FlamePrefab;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        {

        }
    }
}
