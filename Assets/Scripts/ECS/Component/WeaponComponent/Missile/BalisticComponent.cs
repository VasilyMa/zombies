using UnityEngine;

namespace Client 
{
    struct BalisticComponent 
    {
        public Vector3 StartPos;
        public Vector3 EndPos;

        public Vector3 P2;
        public Vector3 P3;

        public float T;         // текущее время
        public float TTarget;   // время до конца полета
    }
}
