using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class InitStartWeaponSystem : IEcsInitSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;


        public void Init (IEcsSystems systems) 
        {
            _state.Value.InvokeStartWeapon();
        }
    }
}
