using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunTurretRequestShootSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<RequestShootEvent, TurretComponent, AimComponent, HealthComponent, TransformComponent>, Exc<InFiretickState>> _filter = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                
            }
        }
    }
}