using Leopotam.EcsLite;
using Statement;
using UnityEngine;
 
public abstract class AbilityBase : ScriptableObject
{
    public abstract float CooldownTime { get; }
    public abstract float CastingTime { get; }  
    public abstract void Resolve(EcsWorld world, BattleState state, int entityCaster);
}
