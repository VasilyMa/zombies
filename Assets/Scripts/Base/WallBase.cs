using UnityEngine;

[CreateAssetMenu(fileName = "WallBase", menuName = "Scriptable Objects/WallBase")]
public class WallBase : ScriptableObject
{
    public string WallName;
    public int Cost;
    public float Health; 
}
