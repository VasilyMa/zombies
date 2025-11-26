using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunDyingSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<DieEvent, TransformComponent>, Exc<PlayerComponent, DeadComponent>> _filter = default; 
        readonly EcsPoolInject<TransformComponent> _transformmPool = default;
        readonly EcsPoolInject<CleanUpEvent> _cleanUpPool = default;
        readonly EcsPoolInject<DeadComponent> _deadPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                _deadPool.Value.Add(entity);

                ref var transformComp = ref _transformmPool.Value.Get(entity);
                transformComp.Transform.gameObject.SetActive(false);
                 
                _cleanUpPool.Value.Add(entity);
            }
        }
    }
}
