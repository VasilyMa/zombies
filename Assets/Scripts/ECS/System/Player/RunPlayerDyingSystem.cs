using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunPlayerDyingSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<DieEvent, TransformComponent, PlayerComponent, AnimateComponent>, Exc<EnemyComponent, DeadComponent>> _filter = default;
        readonly EcsPoolInject<TransformComponent> _transformmPool = default; 
        readonly EcsPoolInject<DeadComponent> _deadPool = default;
        readonly EcsPoolInject<AnimateComponent> _animatePool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                _deadPool.Value.Add(entity);

                ref var animateComp = ref _animatePool.Value.Get(entity);
            }
        }
    }
}