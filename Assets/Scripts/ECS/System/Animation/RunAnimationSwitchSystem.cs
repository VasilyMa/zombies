using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunAnimationSwitchSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<AnimationSwitchStateEvent, AnimateComponent>> _filter = default;
        readonly EcsPoolInject<AnimateComponent> _animatePool = default;
        readonly EcsPoolInject<AnimationSwitchStateEvent> _switchPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var animateComp = ref _animatePool.Value.Get(entity);
                ref var switchComp = ref _switchPool.Value.Get(entity);

                switch (switchComp.Animation)
                {
                    case AnimationSwitchStateEvent.AnimationState.idle:
                        animateComp.Animator.SetBool("IsRun", false);
                        break;
                    case AnimationSwitchStateEvent.AnimationState.run:
                        animateComp.Animator.SetBool("IsRun", true);
                        break;
                }
            }
        }
    }
}