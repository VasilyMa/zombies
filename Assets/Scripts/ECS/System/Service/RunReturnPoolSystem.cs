using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunReturnPoolSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ReturnToPoolEvent>, Exc<PoolComponent>> _filter = default;
        readonly EcsPoolInject<PoolComponent> _pool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                _pool.Value.Add(entity);
            }
        }
    }
}
