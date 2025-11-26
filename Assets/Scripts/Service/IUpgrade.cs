using Leopotam.EcsLite;
using Statement;
using UnityEngine;

public interface IUpgrade
{
    public string Name { get; }
    void Upgrade(EcsWorld world, BattleState state); 
}
