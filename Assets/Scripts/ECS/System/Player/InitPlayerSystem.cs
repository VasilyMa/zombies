using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement; 
using UnityEngine;

namespace Client 
{
    sealed class InitPlayerSystem : IEcsInitSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;
        readonly EcsPoolInject<MovementComponent> _movementPool = default;
        readonly EcsPoolInject<CharacterControllerComponent> _characterPool = default;
        readonly EcsPoolInject<AnimateComponent> _animatePool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<FirePointComponent> _firepointPool = default;
        readonly EcsPoolInject<WeaponHolderComponent> _weaponHolderPool = default;
        readonly EcsPoolInject<UpgradeHolderComponent> _upgradeHolderPool = default;
        readonly EcsPoolInject<PowerComponent> _powerPool = default;
        readonly EcsPoolInject<HasteComponent> _hastePool = default;
        readonly EcsPoolInject<EngineComponent> _enginePool = default;
        readonly EcsPoolInject<RecoveryComponent> _recoveryPool = default;
        readonly EcsPoolInject<DamageHandlerComponent> _damagePool = default;
        readonly EcsPoolInject<BoundsComponent> _boundsPool = default;

        public void Init (IEcsSystems systems) 
        { 
            GameObject playerPrefab = _state.Value.CharacterPlayer;

            if (playerPrefab == null)
            {
                Debug.LogError("Player prefab (CharacterPlayer) не назначен в BattleState.");
                return;
            }

            var playerConfig = ConfigModule.GetConfig<PlayerConfig>();

            var bounds = GameObject.FindFirstObjectByType<PlayerZone>();

            GameObject playerInstance = Object.Instantiate(playerPrefab);

            // Создаём сущность игрока
            var playerEntity = _world.Value.NewEntity();

            // Добавляем всегда PlayerComponent и MovementComponent
            ref var playerComp = ref _playerPool.Value.Add(playerEntity);
            playerComp.Experience = 0;
            playerComp.Money = 10;
            playerComp.NextLevelExperience = playerConfig.Levels[0].NeededExperience;

            ref var damageComp = ref _damagePool.Value.Add(playerEntity);

            ref var recoveryComp = ref _recoveryPool.Value.Add(playerEntity);
            recoveryComp.Value = playerConfig.Recovery;
            recoveryComp.Delay = 1f;

            ref var engineComp = ref _enginePool.Value.Add(playerEntity);
            engineComp.Health = playerConfig.BuildHealth;
            engineComp.Delay = playerConfig.BuildSpeed;

            float damage = playerConfig.Damage * 0.001f;

            ref var powerComp = ref _powerPool.Value.Add(playerEntity);
            powerComp.Init(damage);

            ref var weaponHolderComp = ref _weaponHolderPool.Value.Add(playerEntity);
            weaponHolderComp.WeaponEntities = new System.Collections.Generic.List<int>();

            ref var upgradeHolderComp = ref _upgradeHolderPool.Value.Add(playerEntity);
            upgradeHolderComp.UpgradesEntities = new System.Collections.Generic.List<int>();

            float haste = playerConfig.RapidFire * 0.001f;

            ref var hasteComp = ref _hastePool.Value.Add(playerEntity);
            hasteComp.Init(haste);

            ref var movementComp = ref _movementPool.Value.Add(playerEntity);
            movementComp.Init(playerConfig.MoveSpeed);

            ref var healthComp = ref _healthPool.Value.Add(playerEntity);
            healthComp.Init(playerConfig.Health);
            // Transform всегда есть у GameObject
            ref var transformComp = ref _transformPool.Value.Add(playerEntity);
            transformComp.Transform = playerInstance.transform;

            ref var boundsComp = ref _boundsPool.Value.Add(playerEntity);
            boundsComp.Max = bounds.Max;
            boundsComp.Min = bounds.Min;

            ref var firepointComp = ref _firepointPool.Value.Add(playerEntity);
            firepointComp.FirePoint = playerInstance.transform.Find("FirePoint");

            // CharacterController
            if (playerInstance.TryGetComponent<CharacterController>(out var characterController))
            {
                ref var characterComp = ref _characterPool.Value.Add(playerEntity);
                characterComp.Character = characterController;
            }

            // Animator: ищем вручную по всем дочерним трансформам
            if (TryGetAnimator(playerInstance.transform, out var animator))
            {
                ref var animateComp = ref _animatePool.Value.Add(playerEntity);
                animateComp.Animator = animator;
            }

            transformComp.Transform.gameObject.name = "player";

            _state.Value.AddEntity("player", playerEntity);
        }
         
        // Исправленный вариант поиска аниматора
        private bool TryGetAnimator(Transform parent, out Animator animator)
        {
            animator = parent.GetComponent<Animator>();
            if (animator != null)
                return true;

            for (int i = 0; i < parent.childCount; i++)
            {
                if (TryGetAnimator(parent.GetChild(i), out animator))
                    return true;
            }
            animator = null;
            return false;
        }
    }
}
