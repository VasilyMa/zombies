using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunResolveMissileParticleSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<ResolveMissileEvent, ParticleComponent>> _filter = default;
        readonly EcsPoolInject<ParticleComponent> _particlePool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var particleComp = ref _particlePool.Value.Get(entity);

                int length = particleComp.Particles.Length;

                for (global::System.Int32 i = 0; i < length; i++)
                {
                    particleComp.Particles[i].Clear();
                }
            }
        }
    }
}