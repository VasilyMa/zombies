using UnityEngine;

namespace Client 
{
    struct ThrowFlameEvent 
    {
        public float Tick;
        public float Duration;
        public float Damage;
        public GameObject FlamePrefab;
    }
}
