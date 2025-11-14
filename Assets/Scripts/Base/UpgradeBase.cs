using UnityEngine;
 
public abstract class UpgradeBase : ScriptableObject
{

}

[CreateAssetMenu(fileName = "StatUpgrade", menuName = "Scriptable Objects/Upgrades/StatUpgrade")] 
public class StatBonus : UpgradeBase
{
    public string Name;
    public float BuildProcessBonus;
    public float BuildHealthBonus;
    public float PlayerHealthBonus;
    public float PlayerDamageBonus;
    public float PlayerAttackSpeedBonus;
    public float PlayerHealthRegenerationBonus;
    public float PlayerMoveSpeedBonus;
}

[CreateAssetMenu(fileName = "DoubleShot", menuName = "Scriptable Objects/Upgrades/DoubleShot")] 
public class DoubleShot : UpgradeBase
{
    public string Name;
    public float ChanceToDoubleShot;
}

[CreateAssetMenu(fileName = "FlameShot", menuName = "Scriptable Objects/Upgrades/FlameShot")] 
public class FlameShot : UpgradeBase
{
    public string Name;
    public float DamageValue;
    public float Duration;
    public float Tick;
    public int MaxStack;
}

[CreateAssetMenu(fileName = "FreezeShot", menuName = "Scriptable Objects/Upgrades/FreezeShot")] 
public class FreezeShot : UpgradeBase
{
    public string Name;
    public float Chance;
    public float Duration;
}

[CreateAssetMenu(fileName = "SplitHit", menuName = "Scriptable Objects/Upgrades/SplitHit")] 
public class SplitHit : UpgradeBase
{
    public string Name;
    public float Angle;
    public float DamageValue;
    public int AdditonalMissile;
}

[CreateAssetMenu(fileName = "FractionShot", menuName = "Scriptable Objects/Upgrades/FractionShot")] 
public class FractionShot : UpgradeBase
{
    public string Name;
    public float Radius;
    public float DamageValue; 
} 