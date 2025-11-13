using UnityEngine;

public abstract class TriggerZone : MonoBehaviour
{ 
    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerExit(Collider other);
}
