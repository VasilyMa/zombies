using Client;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Statement;
using System.Collections.Generic; 

public abstract class EcsRunHandler
{
    public EcsWorld World;
    protected EcsSystems _effectSystems;
    protected EcsSystems _upgradeSystems;
    protected EcsSystems _animationSystems;
    protected EcsSystems _turretSystems;
    protected EcsSystems _shootSystems; 
    protected EcsSystems _hitSystems;
    protected EcsSystems _missileSystems;
    protected EcsSystems _commonSystems;
    protected EcsSystems _combatSystems;
    protected EcsSystems _initSystems;
    protected EcsSystems _cleanUpSystems;
    protected EcsSystems _delSystems;

    protected EcsData _data;
    protected List<EcsSystems> _allSystems; 
    protected int _systemsCount;

    public EcsRunHandler(BattleState state)
    {
        World = new EcsWorld();
        _initSystems = new EcsSystems(World, state);
        _upgradeSystems = new EcsSystems(World, state);
        _effectSystems = new EcsSystems(World, state);
        _animationSystems = new EcsSystems(World, state);
        _commonSystems = new EcsSystems(World, state);
        _turretSystems = new EcsSystems(World, state); 
        _hitSystems = new EcsSystems(World, state);
        _missileSystems = new EcsSystems(World, state);
        _combatSystems = new EcsSystems(World, state);
        _shootSystems = new EcsSystems(World, state);
        _cleanUpSystems  = new EcsSystems(World, state);
        _delSystems = new EcsSystems(World, state);

        _data = new EcsData();

        _initSystems
            .Add(new InitCameraSystem())
            .Add(new InitBattleFieldSystem())
            .Add(new InitPlayerSystem())
            .Add(new InitLevelHandlerSystem())
            .Add(new InitWallObjectSystem())
            .Add(new InitTurretObjectSystem())
            .Add(new InitStartWeaponSystem())
            ;

        _upgradeSystems
            .Add(new RunUpgradeRecoverySystem())
            .Add(new RunUpgradeDoubleShotSystem())
            .Add(new RunUpgradeFractionShotSystem())
            .Add(new RunUpgradeStatSystem())
            .Add(new RunUpgradeFlameSystem())
            .Add(new RunUpgradeFreezeSystem())
            .Add(new RunUpgradeSplitShotSystem())

            .DelHere<UpgradeStatEvent>()
            .DelHere<UpgradeFlameEvent>()
            .DelHere<UpgradeDoubleShotEvent>()
            .DelHere<UpgradeFreezeShotEvent>()
            .DelHere<UpgradeRecoveryEvent>()
            .DelHere<UpgradeFractionShotEvent>()
            .DelHere<UpgradeSplitShotEvent>()
            ;

        _hitSystems
            .Add(new RunDamageHitSystem())
            .Add(new RunFlameHitSystem())
            .Add(new RunFreezeHitSystem())
            .Add(new RunSlowHitSystem())

            .Add(new RunResolveHitSystem())
            ;

        _effectSystems
            .Add(new RunBurnEffectSystem())
            .Add(new RunChillEffectSystem())
            .Add(new RunSlowEffectSystem())
            ;


        _missileSystems
            .Add(new RunInvokeMissileSystem()) 
            .Add(new RunInvokeBalisticSystem())
            .DelHere<CompleteShootEvent>()

            .Add(new RunLifetimeMissileSystem())

            .Add(new RunMotionAbilityBlowSystem())
            .Add(new RunMotionMissileSystem())
            .Add(new RunMotionBalisticSystem())
            .Add(new RunDetectionMissileSystem())
            .Add(new RunCollisionMissileSystem())
            .Add(new RunCollisionSniperMissileSystem()) 

            .Add(new RunResolveMissileSplitSystem())
            .Add(new RunResolveMissileDamageSystem())
            .Add(new RunResolveMissileNearSystem())
            .Add(new RunResolveMissileFractionSystem())
            .Add(new RunResolveMissileFlameSystem())
            .Add(new RunResolveMissileFreezeSystem())
            .Add(new RunResolveMissileSlowSystem())
            .Add(new RunResolveMissileExplosionEffectSystem())
            .Add(new RunResolveMissileHitEffectSystem())

            .Add(new RunResolveMissileSystem())
             
            .DelHere<ResolveMissileEvent>()
            .DelHere<MissileCollisionEvent>()
            ;

        _shootSystems
            .Add(new RunTurretRequestShootSystem())
            .Add(new RunRequestShootSystem())

            .Add(new RunRequestSetDoubleSystem())

            .Add(new RunTurretResolveShootSystem())
            //Create missiles for weapon type
            .Add(new RunHandgunResolveShootSystem()) 
            .Add(new RunShotgunResolveShootSystem())
            .Add(new RunSniperRifleResolveShootSystem())
            .Add(new RunAutorifleResolveShootSystem())
            .Add(new RunSubmachineResolveShootSystem())
            .Add(new RunGrenadeResolveShootSystem())

            //Composing missile systems
            .Add(new RunShootSetSplitSystem())
            .Add(new RunShootSetFractionSystem())
            .Add(new RunShootSetSpeedSystem())
            .Add(new RunShootSetFlameSystem())
            .Add(new RunShootSetFreezeSystem())
            .Add(new RunShootSetDamageSystem()) 
            .Add(new RunShootSetLifeSystem())
            .Add(new RunShootSetHitEffectSystem())

            //Complete shoot
            .Add(new RunCompleteShootSystem())

            .DelHere<MissileSetupEvent>()
            .DelHere<RequestShootEvent>()
            ;

        _turretSystems
            .Add(new RunTurretAimSystem())
            .Add(new RunTurretSetTargetSystem())
            .Add(new RunTurretRotateToTargetSystem())

            .DelHere<SetTargetEvent>()
            ;

        _commonSystems
            .Add(new RunReturnPoolSystem()) 

            .Add(new RunLevelHandlerSystem())
            .Add(new RunLevelFinishSystem())
            .Add(new RunLevelLoseSystem())
            .Add(new RunLevelWinSystem())
            .Add(new RunPlayerMovementSystem())
            .Add(new RunEnemyMovementSystem())
            .Add(new RunFastMovementSystem())
            .Add(new RunCombatMovementSystem())

            .Add(new RunPlayerRecoverySystem())

            .Add(new RunEnemySpawnSystem())

            .Add(new RunInFiretickStateSystem())

            .Add(new RunTurretBuildProcessSystem())
            .Add(new RunTurretBuildCompleteSystem())

            .Add(new RunWallBuildProcessSystem())
            .Add(new RunWallBuildCompleteSystem())

            .Add(new RunPlayerLevelUpdateSystem())
            .Add(new RunPlayerLevelUpSystem())
             
            .Add(new RunVFXLifetimeSystem())

            .DelHere<LevelLoseEvent>()
            .DelHere<LevelWinEvent>()
            .DelHere<PlayerExperienceEvent>() 
            .DelHere<SpawnEvent>() 
            .DelHere<ReturnToPoolEvent>()
            .DelHere<CompleteBuildEvent>()
            ;


        _combatSystems 
            .Add(new RunAbilityEnemyCastSystem())
            .Add(new RunAbilityEnemyCastingSystem())
            .Add(new RunAbilityEnemyResolveSystem())
            .Add(new RunAbilityEnemyCooldownSystem())

            .Add(new RunAttackEnemyCastSystem())
            .Add(new RunAttackEnemyCastingSystem())
            .Add(new RunAttackResolveSystem())
            .Add(new RunCombatSystem())
            .Add(new RunActionSystem())

            .Add(new RunDamageResolveSystem())
            .Add(new RunUpdateHealthSystem())

            .Add(new RunPlayerDyingSystem())

            .Add(new RunDyingSystem())  
            .Add(new RunAfterDieExplosionSystem())
            .Add(new RunRewardSystem())

            .DelHere<ResolveAttackEvent>()
            .DelHere<ResolveAbilityEvent>()
            .DelHere<HealthUpdateEvent>()
            .DelHere<DieEvent>()
            ;

        _animationSystems
            .Add(new RunAnimationSwitchSystem())

            .DelHere<AnimationSwitchStateEvent>()
            ;

        _delSystems
            .Add(new RunDisposeSystem<InputMovementState>())
            .Add(new RunDisposeSystem<HitEvent>())
            
            .DelHere<DisposeEvent>()
            ;

        _cleanUpSystems 
            .Add(new RunRecycleSystem<LifetimeComponent>())
            .Add(new RunRecycleSystem<HitAttachComponent>())
            .Add(new RunRecycleSystem<InAttackState>())
            .Add(new RunRecycleSystem<InCombatState>())
            .Add(new RunRecycleSystem<InActionState>())
            .Add(new RunRecycleSystem<AbilityCastingState>())
            .Add(new RunRecycleSystem<CooldownComponent>())
            .Add(new RunRecycleSystem<ChillEffectState>())
            .Add(new RunRecycleSystem<BurnEffectState>())
            .Add(new RunRecycleSystem<SlowEffectState>())
            .Add(new RunRecycleSystem<DamageComponent>())
            .Add(new RunRecycleSystem<SniperBulletComponent>())
            .Add(new RunRecycleSystem<VelocityComponent>())
            .Add(new RunRecycleSystem<FractionAttachComponent>())
            .Add(new RunRecycleSystem<SplitAttachComponent>())
            .Add(new RunRecycleSystem<SlowAttachComponent>())
            .Add(new RunRecycleSystem<NearEffectComponent>())
            .Add(new RunRecycleSystem<FreezeAttachComponent>())
            .Add(new RunRecycleSystem<FlameAttachComponent>())
            .Add(new RunRecycleSystem<ExplodeEffectComponent>())
            .Add(new RunRecycleSystem<MissileMotionState>())
            .Add(new RunRecycleSystem<InFiretickState>())

            .Add(new RunRecyclePoolSystem())
            .DelHere<CleanUpEvent>()
            ;

#if UNITY_EDITOR
        _commonSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif
        _allSystems = new List<EcsSystems>()
        {
            _initSystems,
            _upgradeSystems,
            _missileSystems,
            _hitSystems, 
            _effectSystems,
            _shootSystems,
            _turretSystems,
            _commonSystems,
            _combatSystems,
            _animationSystems,
            _cleanUpSystems,
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