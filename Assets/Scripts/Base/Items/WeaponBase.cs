using Leopotam.EcsLite;
using Statement;
using UnityEngine;
 
public abstract class WeaponBase : ItemBase
{
    public abstract void Init(EcsWorld world, BattleState state);
    public abstract void Upgrade(EcsWorld world, BattleState state, int currentLevels);
    public abstract string GetDescription(int level);
}
