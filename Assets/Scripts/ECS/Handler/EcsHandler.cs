using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Statement;
using System.Collections.Generic;

public abstract class EcsRunHandler
{
    public EcsWorld World;
    protected EcsSystems _commonSystems;
    protected EcsSystems _initSystems;
    protected EcsData _data;
    protected List<EcsSystems> _allSystems; 
    protected int _systemsCount;

    public EcsRunHandler(BattleState state)
    {
        World = new EcsWorld();
        _initSystems = new EcsSystems(World, state);
        _commonSystems = new EcsSystems(World, state);

        _data = new EcsData(); 



#if UNITY_EDITOR
        _commonSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif
        _allSystems = new List<EcsSystems>()
        {
            _initSystems,
            _commonSystems, 
        };
    }

    public virtual void Init()
    {
        for (int i = 0; i < _allSystems.Count; i++)
        {
            var systems = _allSystems[i];
            systems.Inject();
            systems.Init();
        }

        _systemsCount = _allSystems.Count;
    }

    public virtual void Run()
    {
        for (int i = 0; i < _systemsCount; i++)
        {
            _allSystems[i].Run();
        }
    }

    public virtual void FixedRun()
    {

    }

    public virtual void Dispose()
    {
        _allSystems.ForEach(_x => _x.Destroy());
        World.Destroy();
        World = null;
    }
}

public class EcsData
{

}