using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunTurretAimSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<TurretComponent, AimComponent, HealthComponent, TransformComponent>, Exc<SetTargetEvent>> _filter = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<AimComponent> _aimPool = default;
        readonly EcsPoolInject<SetTargetEvent> _targetPool = default;

        float _delay = 0f; 

        readonly int enemyLayerMask = LayerMask.GetMask("Enemy"); 

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                _delay -= Time.deltaTime;

                if (_delay <= 0)
                {
                    _delay = 0.1f;
                    
                    ref var transformComp = ref _transformPool.Value.Get(entity);
                    ref var aimComp = ref _aimPool.Value.Get(entity);
                     
                    _targetPool.Value.Add(entity).Targets = Physics.OverlapSphere(transformComp.Transform.position, aimComp.ViewDistance, enemyLayerMask);
                }
            }
        }
    }
}