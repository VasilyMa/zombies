using UnityEngine;

namespace Client
{ 
    struct UpgradeFlameEvent
    {
        public string Name;
        public float Duration;
        public float Damage;
        public float Tick;
        public GameObject FlamePrefab;
    }
}
