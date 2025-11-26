using Client;
using Leopotam.EcsLite;
using Statement;
using UnityEngine;

public abstract class EnemyBase : ScriptableObject
{
    public string EnemyName;
    [Range(0f, 1f)] public float Weight;
    public float MoveSpeed;
    public float AttackSpeed;
    public float Health;
    public float Attack;
    public float Delay;
    public float Range;
    public int Reward;
    public int Experience;
    public GameObject Prefab;

    public abstract int Init(EcsWorld world, BattleState state, ref SpawnEvent spawnComp);
}
