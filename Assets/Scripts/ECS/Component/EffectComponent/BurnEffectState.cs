using Leopotam.EcsLite;
using Statement; 
using UnityEngine;

namespace Client 
{
    struct BurnEffectState : IRecyclable
    {
        public float Damage;
        public float Delay;
        public float Tick;
        public float Duration;

        public Transform TargetTransform;
        public Transform EffectTransform;
        public string EffectName;

        public void Cleanup(EcsWorld world, BattleState state, int entity)
        { 
            EffectTransform.gameObject.SetActive(false);

            GameObjectPoolService.Release(EffectName, EffectTransform.gameObject);
        }
    }
}
