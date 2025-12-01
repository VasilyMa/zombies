using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunPlayerDyingSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<DieEvent, TransformComponent, PlayerComponent, AnimateComponent>, Exc<DeadComponent>> _filter = default;
        readonly EcsFilterInject<Inc<EnemyComponent>> _enemyFilter = default;
        readonly EcsFilterInject<Inc<WeaponComponent>> _weaponFilter = default;
        readonly EcsFilterInject<Inc<TurretComponent>> _turretFilter = default;
        readonly EcsPoolInject<DeadComponent> _deadPool = default;
        readonly EcsPoolInject<AnimateComponent> _animatePool = default;
        readonly EcsPoolInject<LevelLoseEvent> _losePool = default;
        readonly EcsPoolInject<LockState> _lockPool = default;
        readonly EcsPoolInject<LevelComponent> _levelPool = default;
        readonly EcsPoolInject<ColliderComponent> _colliderPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                _deadPool.Value.Add(entity);

                Debug.Log("Player die!");

                ref var animateComp = ref _animatePool.Value.Get(entity);
                animateComp.Animator.SetBool("IsDie", true);

                ref var colliderComp = ref _colliderPool.Value.Get(entity);
                colliderComp.Collider.enabled = false;

                if (_state.Value.TryGetEntity("level", out int levelEntity))
                {
                    ref var levelComp = ref _levelPool.Value.Get(levelEntity);
                    _state.Value.SetElapsedTime(levelComp.ElapsedTime);

                    foreach (var enemyEntity in _enemyFilter.Value)
                    {
                        ref var animComp = ref _animatePool.Value.Get(enemyEntity);
                        animComp.Animator.SetBool("IsRun", false);
                        _lockPool.Value.Add(enemyEntity);
                    }
                    foreach (var weaponEntity in _weaponFilter.Value)
                    {
                        _lockPool.Value.Add(weaponEntity);
                    }
                    foreach (var turretEntity in _turretFilter.Value)
                    {
                        _lockPool.Value.Add(turretEntity);
                    }

                    _lockPool.Value.Add(levelEntity);
                    _losePool.Value.Add(levelEntity);
                }
            }
        }
    }
}