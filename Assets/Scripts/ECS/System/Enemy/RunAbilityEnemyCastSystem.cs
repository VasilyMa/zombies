using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class RunAbilityEnemyCastSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<AbilityComponent, EnemyComponent, AnimateComponent>, Exc<AbilityCastingState, InActionState, CooldownComponent, DeadComponent>> _filter = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly EcsPoolInject<AbilityCastingState> _castingPool = default;
        readonly EcsPoolInject<AnimateComponent> _animatePool = default;
        readonly EcsPoolInject<InActionState> _inActionPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var abilityComp = ref _abilityPool.Value.Get(entity);
                ref var abilityCastingComp = ref _castingPool.Value.Add(entity);
                ref var inActionComp = ref _inActionPool.Value.Add(entity);

                abilityCastingComp.Delay = abilityComp.AbilityBase.CastingTime; 

                ref var animateComp = ref _animatePool.Value.Get(entity);
                animateComp.Animator.SetTrigger("Cast");

                AnimationClip[] clips = animateComp.Animator.runtimeAnimatorController.animationClips;

                float castDuration = 0f;

                foreach (var clip in clips)
                {
                    if (clip.name == "Cast")
                    {
                        castDuration = clip.length;
                        break;
                    }
                }
                 
                inActionComp.Duration = castDuration;
            }
        }
    }
}