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
        readonly EcsFilterInject<Inc<EnemyComponent, HealthComponent, AttackComponent, MovementComponent, TransformComponent, AnimateComponent, PoolComponent>> _enemyFilter = default;    
        readonly EcsPoolInject<SpawnEvent> _spawnPool = default; 
        readonly EcsPoolInject<PoolComponent> _pool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<MovementComponent> _movementPool = default;
        readonly EcsPoolInject<AttackComponent> _attackPool = default;
        readonly EcsPoolInject<AnimateComponent> _animatePool = default;    
        readonly EcsPoolInject<EnemyComponent> _enemyPool = default;    

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                string entityKey = string.Empty;

                ref var spawnComp = ref _spawnPool.Value.Get(entity);
                var enemyBase = spawnComp.EnemyBase;
                bool isSpawned = false;

                // Буфер для компонентов
                AnimateComponent animateComp;
                AttackComponent attackComp;
                MovementComponent movementComp;
                TransformComponent transformComp;
                HealthComponent healthComp;

                // Попытка найти врага в пуле
                foreach (var enemyEntity in _enemyFilter.Value)
                {
                    ref var poolComp = ref _pool.Value.Get(enemyEntity);

                    if (poolComp.KeyName == enemyBase.EnemyName)
                    {
                        entityKey = enemyEntity.ToString();
                        animateComp = _animatePool.Value.Get(enemyEntity);
                        attackComp = _attackPool.Value.Get(enemyEntity);
                        movementComp = _movementPool.Value.Get(enemyEntity);
                        transformComp = _transformPool.Value.Get(enemyEntity);
                        healthComp = _healthPool.Value.Get(enemyEntity);

                        // Инициализация параметров врага
                        InitEnemyComponents(ref animateComp, ref attackComp, ref movementComp, ref transformComp, ref healthComp, enemyBase, spawnComp);

                        // Активация объекта
                        transformComp.Transform.gameObject.name = entityKey;
                        transformComp.Transform.position = spawnComp.SpawnPoint.position;
                        transformComp.Transform.rotation = new Quaternion(0, 180, 0, 0);
                        transformComp.Transform.gameObject.SetActive(true);

                        _pool.Value.Del(enemyEntity); 
                        isSpawned = true;

                        _state.Value.AddEntity(entityKey, enemyEntity);
                        break; 
                    }
                }

                if (isSpawned) continue;

                // Если враг не найден в пуле — создаём нового
                var newEntityEnemy = _world.Value.NewEntity();

                entityKey = newEntityEnemy.ToString();

                var instance = GameObject.Instantiate(enemyBase.Prefab, spawnComp.SpawnPoint.position, Quaternion.identity);

                instance.gameObject.name = entityKey;

                animateComp = new AnimateComponent();
                attackComp = new AttackComponent();
                movementComp = new MovementComponent();
                transformComp = new TransformComponent { Transform = instance.transform };
                healthComp = new HealthComponent();

                // Инициализация параметров врага
                InitEnemyComponents(ref animateComp, ref attackComp, ref movementComp, ref transformComp, ref healthComp, enemyBase, spawnComp);

                // Добавление компонентов в ECS
                _animatePool.Value.Add(newEntityEnemy) = animateComp;
                _attackPool.Value.Add(newEntityEnemy) = attackComp;
                _movementPool.Value.Add(newEntityEnemy) = movementComp;
                _transformPool.Value.Add(newEntityEnemy) = transformComp;
                _healthPool.Value.Add(newEntityEnemy) = healthComp;
                
                ref var enemyComp = ref _enemyPool.Value.Add(newEntityEnemy);
                enemyComp.EnemyName = enemyBase.EnemyName;

                transformComp.Transform.position = spawnComp.SpawnPoint.position;
                transformComp.Transform.rotation = new Quaternion(0, 180, 0, 0);
                transformComp.Transform.gameObject.SetActive(true);

                _state.Value.AddEntity(entityKey, newEntityEnemy);
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
            EnemyBase enemyBase,
            SpawnEvent spawnComp)
        {
            float amplifier = spawnComp.Amplifier;

            float health = enemyBase.Health + (enemyBase.Health * amplifier);
            healthComp.Init(health);

            float moveSpeed = enemyBase.MoveSpeed + (enemyBase.MoveSpeed * amplifier);
            movementComp.MoveSpeed = moveSpeed;

            float attackDamage = enemyBase.Attack + (enemyBase.Attack * amplifier);
            attackComp.Damage = attackDamage;

            // transformComp.Transform.position задаётся отдельно при активации/создании
        }
    }
}
