using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunCompleteShootSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<MissileSetupEvent>> _filter = default;
        readonly EcsPoolInject<CompleteShootEvent> _completePool = default;
        readonly EcsPoolInject<MissileSetupEvent> _missilePool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var missileComp = ref _missilePool.Value.Get(entity);

                foreach (var missileEntity in missileComp.MissileEntity)
                { 
                    _completePool.Value.Add(missileEntity); 
                }

                ListPool<int>.Release(missileComp.MissileEntity);
            }
        }
    }
}
