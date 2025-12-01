using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client
{
    sealed class RunAttackEnemyCastSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsFilterInject<Inc<AttackComponent, TransformComponent, InCombatState, AnimateComponent>, Exc<InAttackState, InActionState, LockState>> _filter = default;
        readonly EcsPoolInject<InCombatState> _combatPool = default;
        readonly EcsPoolInject<InAttackState> _inAttackPool = default;
        readonly EcsPoolInject<InActionState> _inActionPool = default;
        readonly EcsPoolInject<AttackComponent> _attackPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<AnimateComponent> _animatePool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<InCombatState> _combatState = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var combatComp = ref _combatPool.Value.Get(entity);

                ref var targetTransformComp = ref _transformPool.Value.Get(combatComp.TargetEntity);
                ref var transformComp = ref _transformPool.Value.Get(entity);

                if (!_healthPool.Value.Has(combatComp.TargetEntity))
                { 
                    _combatState.Value.Del(entity);
                    continue;
                }

                if (Vector3.Distance(targetTransformComp.Transform.position, transformComp.Transform.position) <= combatComp.DistanceToAttack + 0.5f)
                {
                    ref var attackComp = ref _attackPool.Value.Get(entity);
                    ref var inAttackComp = ref _inAttackPool.Value.Add(entity);
                    inAttackComp.Delay = attackComp.Delay;

                    ref var animateComp = ref _animatePool.Value.Get(entity);
                    animateComp.Animator.SetTrigger("Attack");

                    AnimationClip[] clips = animateComp.Animator.runtimeAnimatorController.animationClips;

                    float actionDuration = 0f;

                    foreach (var clip in clips)
                    {
                        if (clip.name == "Attack")
                        {
                            actionDuration = clip.length;
                            break;
                        }
                    }

                    _inActionPool.Value.Add(entity).Duration = actionDuration;
                }
                else
                {
                    _combatState.Value.Del(entity);
                }
            }
        }
    }
}