using System;
using UnityEngine;

public class TurretZone : TriggerZone
{
    public int Entity;
    public event Action<int, Collider> TriggerEntered;
    public event Action<int, Collider> TriggerExited;

    public GameObject turretObject;
      
    public override void OnTriggerEnter(Collider other)
    {
        TriggerEntered?.Invoke(Entity, other);
    }

    public override void OnTriggerExit(Collider other)
    {
        TriggerExited?.Invoke(Entity, other);
    }

    private void OnDestroy()
    {
        TriggerExited = null;
        TriggerEntered = null;
    }
}
