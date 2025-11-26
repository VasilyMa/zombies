using Leopotam.EcsLite;
using Statement;
using UnityEngine;

namespace Client 
{
    struct HitEffectComponent : IRecyclable
    {
        public GameObject Prefab;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        {

        }
    }
}
