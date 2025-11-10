using Client; 
using Leopotam.EcsLite;
using System.Collections.Generic; 

namespace Statement
{
    public class BattleState : State
    {
        public static new BattleState Instance
        {
            get
            {
                return (BattleState)State.Instance;
            }
        } 

        public EntityBase PlayerEntityBase;
        protected EcsRunHandler EcsHandler;
        protected Dictionary<string, EcsPackedEntity> _ecsMap = new();
        public override void Awake()
        {
            base.Awake();

            EcsHandler = new MainEcsHandler(this);
        }
        public override void Start()
        {
            EcsHandler.Init();
        }
        public override void Update() => EcsHandler.Run();
        public override void FixedUpdate() => EcsHandler.FixedRun();
        public override void OnDestroy()
        {
            base.OnDestroy();   
            EcsHandler.Dispose();
        }
         

        public virtual void AddEntity(string localKey, int entity)
        {
            var packed = EcsHandler.World.PackEntity(entity);

            if (!string.IsNullOrEmpty(localKey) && !_ecsMap.ContainsKey(localKey))
                _ecsMap[localKey] = packed;
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
           
    }
}