using UnityEngine;

namespace Client 
{
    struct TurretComponent 
    {
        public Transform FirePoint;
        public GameObject MissilePrefab;
        public float CurrentProgress;
        public float TargetProgress;
        public float RotationSpeed;
    }
}
