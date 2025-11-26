using Leopotam.EcsLite;
using Statement;
using UnityEngine;

namespace Client 
{
    struct ExplodeEffectComponent : IRecyclable
    {
        public float RadiusArea;
        public GameObject ExplosionPrefab;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        {

        }
    }
}
