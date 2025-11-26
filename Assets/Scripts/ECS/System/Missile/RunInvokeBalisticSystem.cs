using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client
{
    sealed class RunInvokeBalisticSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;

        readonly EcsFilterInject<Inc<MissileComponent, CompleteShootEvent, TransformComponent, BalisticComponent, VelocityComponent>> _filter = default;

        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<BalisticComponent> _balisticPool = default;
        readonly EcsPoolInject<VelocityComponent> _velocityPool = default;
        readonly EcsPoolInject<MissileMotionState> _motionStatePool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var transformComp = ref _transformPool.Value.Get(entity);
                ref var bal = ref _balisticPool.Value.Get(entity);
                ref var vel = ref _velocityPool.Value.Get(entity);
                 
                transformComp.Transform.gameObject.SetActive(true); 
                transformComp.Transform.position = bal.StartPos;

                float distance = Vector3.Distance(bal.StartPos, bal.EndPos);

                // t и t_target
                bal.T = 0f;
                bal.TTarget = distance / vel.Speed;

                // p2 и p3
                float multi = Random.Range(2.5f, 5f);
                bal.P2 = bal.StartPos + Vector3.up * multi;
                bal.P3 = bal.EndPos + Vector3.up * multi;

                // ----------------------------
                // 3. Переключаем состояние на "летит"
                // ----------------------------
                if (!_motionStatePool.Value.Has(entity))
                    _motionStatePool.Value.Add(entity); 
            }
        }
    }
}
