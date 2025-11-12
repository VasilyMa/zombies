using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunDyingSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<DieEvent, TransformComponent>> _filter = default;
        readonly EcsPoolInject<DeadComponent> _deadPool = default;
        readonly EcsPoolInject<ReturnToPoolEvent> _returnPool = default;
        readonly EcsPoolInject<TransformComponent> _transformmPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var transformComp = ref _transformmPool.Value.Get(entity);
                transformComp.Transform.gameObject.SetActive(false);

                _deadPool.Value.Add(entity);
                _returnPool.Value.Add(entity);
            }
        }
    }
}
