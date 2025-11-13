using Client;
using Leopotam.EcsLite;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Statement
{
    public class BattleState : State
    {
        public GameObject CharacterPlayer;
        public static new BattleState Instance
        {
            get
            { 
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<BattleState>();
                }
                return (BattleState)_instance;
            }
        }

        protected EcsRunHandler EcsHandler;
        protected Dictionary<string, EcsPackedEntity> _ecsMap = new();
        [SerializeField] protected LevelBase _levelData;
        public LevelBase LevelData => _levelData;

        public override void Awake()
        {
            base.Awake();

            if (_instance == null) _instance = this; 

            EcsHandler = new MainEcsHandler(this);

            UIModule.Inject(EcsHandler.World, this);

            if (SceneBus.TryGet<LevelBase>(out var levelData))
            { 
                _levelData = levelData;

                EcsHandler = new MainEcsHandler(this);

                UIModule.Inject(EcsHandler.World, this);
            }

        }

        public override void Start()
        {
            EcsHandler.Init();

            if (UIModule.OpenCanvas<BattleCanvas>(out var battleCanvas))
            {
                if (battleCanvas.TryOpenPanel<BattlePanel>(out var panel))
                {
                    // Можно добавить инициализацию панели, если требуется
                }
            }
        }

        public override void Update() => EcsHandler.Run();
        public override void FixedUpdate() => EcsHandler.FixedRun();

        public override void OnDestroy()
        {
            base.OnDestroy();
            EcsHandler.Dispose(); 
            if (_instance == this)
                _instance = null;
        }

        public virtual void AddEntity(string localKey, int entity)
        {
            var packed = EcsHandler.World.PackEntity(entity);

            if (!string.IsNullOrEmpty(localKey) && !_ecsMap.ContainsKey(localKey))
                _ecsMap[localKey] = packed;
        }

        public virtual void RemoveEntity(string localKey)
        {
            if (!string.IsNullOrEmpty(localKey) && _ecsMap.ContainsKey(localKey))
            {
                _ecsMap.Remove(localKey);
            }
        }

        public virtual bool TryGetEntity(string key, out EcsPackedEntity packedEntity)
        {
            packedEntity = default;

            if (string.IsNullOrEmpty(key)) return false;

            return _ecsMap.TryGetValue(key, out packedEntity);
        }

        public virtual bool TryGetEntity(string key, out int unpackedEntity)
        {
            if (TryGetEntity(key, out EcsPackedEntity packed) && packed.Unpack(EcsHandler.World, out int entity))
            {
                unpackedEntity = entity;
                return true;
            }

            unpackedEntity = -1;
            return false;
        }

        public virtual void InvokeStartWeapon()
        {
            var weaponConfig = ConfigModule.GetConfig<WeaponConfig>();

            var starterPack = weaponConfig.GetStarterPack();

            if (UIModule.TryGetCanvas<BattleCanvas>(out var battleCanvas))
            {
                if (battleCanvas.TryGetPanel<BattlePanel>(out var panel))
                {
                    panel.OpenWindow<StartWeaponWindow>(starterPack.Cast<object>().ToArray());
                }
            }
        }
    }
}