using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class InitBattleFieldSystem : IEcsInitSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsPoolInject<BattleZoneComponent> _battlePool = default;

        public void Init (IEcsSystems systems) 
        {
            var battleField = GameObject.FindFirstObjectByType<BattleZone>();

            var battleEntity = _world.Value.NewEntity();

            ref var battleComp = ref _battlePool.Value.Add(battleEntity);
            battleComp.BattleZone = battleField;
            battleComp.Max = battleField.Max;
            battleComp.Min = battleField.Min;

            _state.Value.AddEntity("battle", battleEntity);
        }
    }
}
