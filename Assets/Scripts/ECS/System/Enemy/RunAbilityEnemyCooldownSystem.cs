using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunAbilityEnemyCooldownSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<CooldownComponent>, Exc<DeadComponent>> _filter = default;
        readonly EcsPoolInject<CooldownComponent> _cooldownPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var cooldownComp = ref _cooldownPool.Value.Get(entity);
                cooldownComp.RemainingTime -= Time.deltaTime;

                if (cooldownComp.RemainingTime <= 0)
                {
                    _cooldownPool.Value.Del(entity);
                }
            }
        }
    }
}