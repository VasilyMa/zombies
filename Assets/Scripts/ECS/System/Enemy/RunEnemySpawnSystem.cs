using Leopotam.EcsLite; 
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine; 
namespace Client 
{
    sealed class RunEnemySpawnSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<SpawnEvent>> _filter = default;
        readonly EcsPoolInject<SpawnEvent> _spawnPool = default; 
        readonly EcsPoolInject<PoolComponent> _pool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<MovementComponent> _movementPool = default;
        readonly EcsPoolInject<AttackComponent> _attackPool = default;
        readonly EcsPoolInject<AnimateComponent> _animatePool = default;    
        readonly EcsPoolInject<RewardComponent> _reardPool = default;
        readonly EcsPoolInject<DeadComponent> _deadPool = default;
        readonly EcsPoolInject<AnimationSwitchStateEvent> _switchPool = default;
        readonly EcsPoolInject<ExplosionComponent> _explosionPool = default;
        readonly EcsPoolInject<CombatComponent> _combatPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                string entityKey = string.Empty;
                int enemyEntity = -1;
                ref var spawnComp = ref _spawnPool.Value.Get(entity);
                var enemyBase = spawnComp.EnemyBase; 
                 
                if (EntityPoolService.TryGet(enemyBase.EnemyName, out enemyEntity))
                {
                    entityKey = enemyEntity.ToString();
                    ref var animateComp = ref _animatePool.Value.Get(enemyEntity);
                    ref var attackComp = ref _attackPool.Value.Get(enemyEntity);
                    ref var movementComp = ref _movementPool.Value.Get(enemyEntity);
                    ref var transformComp = ref _transformPool.Value.Get(enemyEntity);
                    ref var healthComp = ref _healthPool.Value.Get(enemyEntity);
                    ref var rewardComp = ref _reardPool.Value.Get(enemyEntity);

                    // Инициализация параметров врага
                    InitEnemyComponents(ref animateComp, ref attackComp, ref movementComp, ref transformComp, ref healthComp, ref rewardComp, enemyBase, spawnComp);

                    // Активация объекта
                    transformComp.Transform.gameObject.name = entityKey;
                    transformComp.Transform.position = spawnComp.SpawnPoint;
                    transformComp.Transform.rotation = new Quaternion(0, 180, 0, 0);
                    transformComp.Transform.gameObject.SetActive(true);

                    if (_explosionPool.Value.Has(entity))
                    { 
                        ref var explosionComp = ref _explosionPool.Value.Get(entity);
                        explosionComp.Damage = attackComp.Damage * explosionComp.Multiplier; 
                    }

                    if (!_switchPool.Value.Has(enemyEntity)) _switchPool.Value.Add(enemyEntity).Animation = AnimationSwitchStateEvent.AnimationState.run;
                    _state.Value.AddEntity(entityKey, enemyEntity); 
                    _deadPool.Value.Del(enemyEntity);
                }
                else
                {
                    // Если враг не найден в пуле — создаём нового
                    enemyEntity = enemyBase.Init(_world.Value, _state.Value, ref spawnComp);
                    _combatPool.Value.Add(enemyEntity).Delay = 0.01f;
                    if (!_switchPool.Value.Has(enemyEntity)) _switchPool.Value.Add(enemyEntity).Animation = AnimationSwitchStateEvent.AnimationState.run;
                }  
            }
        }

        /// <summary>
        /// Инициализация всех компонентов врага.
        /// </summary>
        private void InitEnemyComponents(
            ref AnimateComponent animateComp,
            ref AttackComponent attackComp,
            ref MovementComponent movementComp,
            ref TransformComponent transformComp,
            ref HealthComponent healthComp,
            ref RewardComponent rewardComp,
            EnemyBase enemyBase,
            SpawnEvent spawnComp)
        {
            float amplifier = spawnComp.Amplifier;

            float health = enemyBase.Health + (enemyBase.Health * amplifier);
            healthComp.Init(health);

            float moveSpeed = enemyBase.MoveSpeed + (enemyBase.MoveSpeed * amplifier);
            movementComp.Init(moveSpeed);

            float attackDamage = enemyBase.Attack + (enemyBase.Attack * amplifier);
            attackComp.Damage = attackDamage;

            rewardComp.Reward = enemyBase.Reward;
            rewardComp.Experience = enemyBase.Experience;  
        }
    }
}
