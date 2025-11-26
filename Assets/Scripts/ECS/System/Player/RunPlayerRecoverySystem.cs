using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunPlayerRecoverySystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<RecoveryComponent, HealthComponent>> _filter = default;
        readonly EcsPoolInject<RecoveryComponent> _recoveryPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var recoveryComp = ref _recoveryPool.Value.Get(entity);

                recoveryComp.CurrentDelay -= Time.deltaTime;

                if (recoveryComp.CurrentDelay <= 0)
                { 
                    ref var healthComp = ref _healthPool.Value.Get(entity);
                    healthComp.Add(recoveryComp.Value);

                    recoveryComp.CurrentDelay = recoveryComp.Delay;
                }
            }
        }
    }
}