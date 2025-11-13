using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildConfig", menuName = "Config/BuildConfig")]
public class BuildConfig : Config
{
    public List<TurretBase> TurretBases;
    public List<WallBase> WallBases;

    public override IEnumerator Init()
    {
        yield return null;
    }
}
