using UnityEngine;

namespace Client 
{
    struct WeaponComponent 
    {
        public int OwnerEntity; 
        public Transform FirePoint;
        public GameObject MissilePrefab;
        public GameObject MuzzlePrefab;
    }
}
