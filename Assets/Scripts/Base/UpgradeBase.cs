using Client;
using Leopotam.EcsLite;
using Statement;
using UnityEngine;
 
public abstract class UpgradeBase : ScriptableObject, IUpgrade
{ 
    public string Name; 
    string IUpgrade.Name => Name;

    public virtual void Apply(EcsWorld world, BattleState state)
    {
        if (state.TryGetEntity("player", out int playerEntity))
        {
            ref var upgradeHolderComp = ref world.GetPool<UpgradeHolderComponent>().Get(playerEntity);

            foreach (var entity in upgradeHolderComp.UpgradesEntities)
            { 
                if (world.GetPool<UpgradeComponent>().Get(entity).KeyName == Name) return;  
            }

            var upEntity = world.NewEntity();

            ref var upgradeComp = ref world.GetPool<UpgradeComponent>().Add(upEntity);
            upgradeComp.KeyName = Name;

            upgradeHolderComp.UpgradesEntities.Add(upEntity);
        }
    }
    public abstract void Apply(EcsWorld world, BattleState state, int level, int entity); 
    public abstract void Upgrade(EcsWorld world, BattleState state);
}
