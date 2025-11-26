using Leopotam.EcsLite;
using Statement;
using UnityEngine;
 
public abstract class WeaponBase : ItemBase, IUpgrade
{
    string IUpgrade.Name => Name;
    public GameObject HitPrefab;
    public abstract void Init(EcsWorld world, BattleState state);
    public abstract void Upgrade(EcsWorld world, BattleState state, int entity);
    public abstract string GetDescription(int level); 
    public abstract void Upgrade(EcsWorld world, BattleState state); 
}
