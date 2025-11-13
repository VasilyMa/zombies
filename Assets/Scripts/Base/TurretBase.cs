using UnityEngine;

[CreateAssetMenu(fileName = "TurretBase", menuName = "Scriptable Objects/TurretBase")]
public class TurretBase : ScriptableObject, ISerializationCallbackReceiver
{
    public string TurretName;
    public int Cost;
    public float Health;
    public float Damage;
    public float Rapidfire;
    public float Speed;
    public float RotationSpeed;
    public float ViewDistance;
    public float Angle;
    public GameObject MissilePrefab;
    [ReadOnlyInspector] public float DPS;
    public int MissileCount;

    public void OnAfterDeserialize()
    { 

    }

    public void OnBeforeSerialize()
    {
        if (Rapidfire != 0)
        {
            DPS = Damage / Rapidfire;
        }
    }
}
