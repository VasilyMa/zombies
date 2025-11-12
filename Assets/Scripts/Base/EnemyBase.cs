using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Entites/Enemy")]
public class EnemyBase : ScriptableObject
{
    public string EnemyName;
    [Range(0f, 1f)] public float Weight;
    public float MoveSpeed;
    public float AttackSpeed;
    public float Health;
    public float Attack;
    public GameObject Prefab;
}
