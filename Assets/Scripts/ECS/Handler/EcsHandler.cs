using Client;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Statement;
using System.Collections.Generic;

public abstract class EcsRunHandler
{
    public EcsWorld World;
    protected EcsSystems _shootSystems;
    protected EcsSystems _takeSystems;
    protected EcsSystems _hitSystems;
    protected EcsSystems _missileSystems;
    protected EcsSystems _commonSystems;
    protected EcsSystems _combatSystems;
    protected EcsSystems _initSystems;
    protected EcsSystems _delSystems;

    protected EcsData _data;
    protected List<EcsSystems> _allSystems; 
    protected int _systemsCount;

    public EcsRunHandler(BattleState state)
    {
        World = new EcsWorld();
        _initSystems = new EcsSystems(World, state);
        _commonSystems = new EcsSystems(World, state);
        _takeSystems = new EcsSystems(World, state);
        _hitSystems = new EcsSystems(World, state);
        _missileSystems = new EcsSystems(World, state);
        _combatSystems = new EcsSystems(World, state);
        _shootSystems = new EcsSystems(World, state);
        _delSystems = new EcsSystems(World, state);

        _data = new EcsData();

        _initSystems
            .Add(new InitPlayerSystem())
            .Add(new InitLevelHandlerSystem())
            .Add(new InitStartWeaponSystem())
            ;

        _hitSystems
            .Add(new RunDamageHitSystem())

            .Add(new RunResolveHitSystem())
            ;

        _takeSystems
            .Add(new RunTakeDamageSystem())
            
            .DelHere<TakeDamageEvent>()
            ;

        _missileSystems
            .Add(new RunInvokeMissileSystem())
            .Add(new RunMotionMissileSystem())
            .Add(new RunDetectionMissileSystem())
            .Add(new RunCollisionMissileSystem())

            .Add(new RunResolveMissileDamageSystem())

            .Add(new RunResolveMissileSystem())

            .DelHere<ResolveMissileEvent>()
            .DelHere<CompleteShootEvent>()
            ;

        _shootSystems
            .Add(new RunRequestShootSystem())
            //Create missiles for weapon type
            .Add(new RunHandgunResolveShootSystem()) 

            //Composing missile systems
            .Add(new RunWeaponSetDamageSystem())
            .Add(new RunWeaponSetSpeedSystem())

            //Complete shoot
            .Add(new RunCompleteShootSystem())

            .DelHere<MissileSetupEvent>()
            .DelHere<RequestShootEvent>()
            ;

        _commonSystems
            .Add(new RunReturnPoolSystem()) 

            .Add(new RunLevelHandlerSystem())
            .Add(new RunPlayerMovementSystem())
            .Add(new RunEnemyMovementSystem())

            .Add(new RunEnemySpawnSystem())

            .Add(new RunInFiretickStateSystem())

            .DelHere<SpawnEvent>() 
            .DelHere<ReturnToPoolEvent>()
            ;


        _combatSystems
            .Add(new RunUpdateHealthSystem())

            .Add(new RunDyingSystem())

            .Add(new RunEnemyDeadSystem())
            .Add(new RunMissileDeadSystem())

            .DelHere<DieEvent>()
            ;

        _delSystems
            .Add(new RunDisposeSystem<InputMovementState>())
            .Add(new RunDisposeSystem<HitEvent>())
            
            .DelHere<DisposeEvent>()
            ;

#if UNITY_EDITOR
        _commonSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif
        _allSystems = new List<EcsSystems>()
        {
            _initSystems,
            _hitSystems,
            _takeSystems,
            _missileSystems,
            _shootSystems,
            _commonSystems,
            _combatSystems,
            _delSystems
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