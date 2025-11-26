using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunInFiretickStateSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<InFiretickState>> _filter = default;
        readonly EcsPoolInject<InFiretickState> _statePool = default;
        readonly EcsPoolInject<CleanUpEvent> _cleanUpPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var firetickComp = ref _statePool.Value.Get(entity);
                firetickComp.RemainingTime -= Time.deltaTime;

                if (firetickComp.RemainingTime < 0)
                {
                    _cleanUpPool.Value.Add(entity);
                }
            }
        }
    }
}
