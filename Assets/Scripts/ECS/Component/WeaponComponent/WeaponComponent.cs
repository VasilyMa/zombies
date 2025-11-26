using UnityEngine;

namespace Client 
{
    struct WeaponComponent 
    {
        public int Level;
        public int MaxLevel;
        public int OwnerEntity; 
        public string Name; 
        public Transform FirePoint;
        public GameObject MissilePrefab;
        public GameObject MuzzlePrefab;
    }
}
