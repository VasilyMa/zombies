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
        public void Init (IEcsSystems systems) 
        { 
            GameObject playerPrefab = _state.Value.CharacterPlayer;

            if (playerPrefab == null)
            {
                Debug.LogError("Player prefab (CharacterPlayer) не назначен в BattleState.");
                return;
            }

            GameObject playerInstance = Object.Instantiate(playerPrefab);

            // Создаём сущность игрока
            var playerEntity = _world.Value.NewEntity();

            // Добавляем всегда PlayerComponent и MovementComponent
            ref var playerComp = ref _playerPool.Value.Add(playerEntity);
            ref var movementComp = ref _movementPool.Value.Add(playerEntity);
            movementComp.MoveSpeed = 5f;
            ref var healthComp = ref _healthPool.Value.Add(playerEntity);
            healthComp.Init(100);
            // Transform всегда есть у GameObject
            ref var transformComp = ref _transformPool.Value.Add(playerEntity);
            transformComp.Transform = playerInstance.transform;

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
