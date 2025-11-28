using UnityEngine;

namespace Client 
{
    struct TurretComponent 
    {
        public Transform FirePoint;
        public Transform TurretObject;
        public GameObject MissilePrefab;
        public float CurrentProgress;
        public float TargetProgress;
        public float RotationSpeed;
    }
}
