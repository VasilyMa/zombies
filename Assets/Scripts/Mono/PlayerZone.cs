using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerZone : TriggerZone
{
    private BoxCollider _box;
    public event Action<Collider> TriggerEntered;
    public event Action<Collider> TriggerExited;
    public Bounds ZoneBounds { get; private set; }
    public Vector2 Min { get; private set; }
    public Vector2 Max { get; private set; }

    private void Awake()
    {
        InitBounds();
    }

    private void OnDestroy()
    {
        TriggerEntered = null;
        TriggerExited = null;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        InitBounds();
    }
#endif

    private void InitBounds()
    {
        _box = GetComponent<BoxCollider>();
        _box.isTrigger = true;

        ZoneBounds = _box.bounds;

        // Только XZ-плоскость (как для движения юнитов)
        Min = new Vector2(ZoneBounds.min.x, ZoneBounds.min.z);
        Max = new Vector2(ZoneBounds.max.x, ZoneBounds.max.z);
    }

    // Вызывается при попадании юнита в зону
    public override void OnTriggerEnter(Collider other)
    {
        // Пример (можешь заменить на ECS-событие):
        // if (other.TryGetComponent(out Unit u))
        //     u.SetBattleZone(this);
    }

    // Вызывается при выходе из зоны
    public override void OnTriggerExit(Collider other)
    {
        // Пример:
        // if (other.TryGetComponent(out Unit u))
        //     u.ClearBattleZone();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_box == null) _box = GetComponent<BoxCollider>();
        if (_box == null) return;

        Gizmos.color = new Color(0.3f, 0.8f, 1f, 0.25f);
        Gizmos.DrawCube(_box.bounds.center, _box.bounds.size);

        Gizmos.color = new Color(0.1f, 0.4f, 1f, 1f);
        Gizmos.DrawWireCube(_box.bounds.center, _box.bounds.size);
    }
#endif
}