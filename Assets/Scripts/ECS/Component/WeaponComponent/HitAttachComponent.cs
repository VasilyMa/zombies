using Leopotam.EcsLite;
using Statement;
using UnityEngine;

namespace Client 
{
    struct HitAttachComponent : IRecyclable
    {
        public GameObject Prefab;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        {

        }
    }
}
