using Client;
using Leopotam.EcsLite;
using Statement;
using UnityEngine; 

[CreateAssetMenu(fileName = "AbilityBlowBase", menuName = "Scriptable Objects/AbilityBlowBase")]
public class AbilityBlowBase : AbilityBase
{
    public GameObject PrefabMissile;
    public GameObject PrefabExplosion;
    public float DamageValue;
    public float Radius;
    public float Speed;
    public float Cooldown;  
    public float Casting;
    public float Duration;

    public override float CooldownTime => Cooldown; 
    public override float CastingTime => Casting; 

    public override void Resolve(EcsWorld world, BattleState state, int entityCaster)
    {
        if (world.GetPool<AttackComponent>().Has(entityCaster))
        {
            if (state.TryGetEntity("player", out int playerEntity))
            {
                ref var attackComp = ref world.GetPool<AttackComponent>().Get(entityCaster);
                float damage = attackComp.Damage * (1 + attackComp.Modifier);
                damage += damage * DamageValue;

                ref var casterTransformComp = ref world.GetPool<TransformComponent>().Get(entityCaster);
                ref var targetTransformComp = ref world.GetPool<TransformComponent>().Get(playerEntity);
                var targetPos = targetTransformComp.Transform.position;

                int missileEntity = -1;

                if (EntityPoolService.TryGet(PrefabMissile.name, out missileEntity))
                {
                    ref var transformComp = ref world.GetPool<TransformComponent>().Get(missileEntity);
                    ref var explodeComp = ref world.GetPool<ExplodeEffectComponent>().Add(missileEntity);
                    explodeComp.RadiusArea = Radius;
                    explodeComp.ExplosionPrefab = PrefabExplosion;
                    ref var damageComp = ref world.GetPool<DamageComponent>().Add(missileEntity);
                    damageComp.Value = damage;
                    ref var motioComp = ref world.GetPool<MissileMotionState>().Add(missileEntity);
                    ref var velocityComp = ref world.GetPool<VelocityComponent>().Add(missileEntity);
                    velocityComp.Speed = Speed;
                    ref var balisticComp = ref world.GetPool<BalisticComponent>().Get(missileEntity);
                    balisticComp.StartPos = casterTransformComp.Transform.position + Vector3.up;
                    balisticComp.EndPos = targetPos;

                    float distance = Vector3.Distance(balisticComp.StartPos, balisticComp.EndPos);
                    // t è t_target
                    balisticComp.T = 0f;
                    balisticComp.TTarget = distance / Speed;

                    // p2 è p3
                    float multi = Random.Range(2.5f, 5f);
                    balisticComp.P2 = balisticComp.StartPos + Vector3.up * multi;
                    balisticComp.P3 = balisticComp.EndPos + Vector3.up * multi;

                    transformComp.Transform.position = balisticComp.StartPos;
                    transformComp.Transform.gameObject.SetActive(true);
                }
                else
                {
                    missileEntity = world.NewEntity();

                    world.GetPool<PoolComponent>().Add(missileEntity).KeyName = PrefabMissile.name;
                    var missileInstance = GameObject.Instantiate(PrefabMissile, casterTransformComp.Transform.position + Vector3.up, Quaternion.identity);
                    ref var missileComp = ref world.GetPool<MissileComponent>().Add(missileEntity);
                    missileComp.KeyName = PrefabMissile.name;
                    ref var transformComp = ref world.GetPool<TransformComponent>().Add(missileEntity);
                    transformComp.Transform = missileInstance.transform;
                    ref var explodeComp = ref world.GetPool<ExplodeEffectComponent>().Add(missileEntity);
                    explodeComp.RadiusArea = Radius;
                    explodeComp.ExplosionPrefab = PrefabExplosion;
                    ref var damageComp = ref world.GetPool<DamageComponent>().Add(missileEntity);
                    damageComp.Value = damage;
                    ref var motioComp = ref world.GetPool<MissileMotionState>().Add(missileEntity);
                    ref var velocityComp = ref world.GetPool<VelocityComponent>().Add(missileEntity);
                    velocityComp.Speed = Speed;
                    ref var balisticComp = ref world.GetPool<BalisticComponent>().Add(missileEntity);
                    balisticComp.StartPos = casterTransformComp.Transform.position;
                    balisticComp.EndPos = targetPos; 
                    ref var paticleComp = ref world.GetPool<ParticleComponent>().Add(missileEntity);
                    paticleComp.Particles = missileInstance.GetComponentsInChildren<ParticleSystem>(); 

                    float distance = Vector3.Distance(balisticComp.StartPos, balisticComp.EndPos);
                    // t è t_target
                    balisticComp.T = 0f;
                    balisticComp.TTarget = distance / Speed;

                    // p2 è p3
                    float multi = Random.Range(2.5f, 5f);
                    balisticComp.P2 = balisticComp.StartPos + Vector3.up * multi;
                    balisticComp.P3 = balisticComp.EndPos + Vector3.up * multi; 

                    transformComp.Transform.position = balisticComp.StartPos;
                    transformComp.Transform.gameObject.SetActive(true);
                }
            }
        } 
    } 
}
