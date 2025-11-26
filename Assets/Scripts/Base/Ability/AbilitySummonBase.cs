using Client;
using Leopotam.EcsLite;
using Statement;
using UnityEngine;
using UnityEngine.LightTransport;

[CreateAssetMenu(fileName = "AbilitySummonBase", menuName = "Scriptable Objects/AbilitySummonBase")]
public class AbilitySummonBase : AbilityBase
{
    [SerializeField] private float cooldown;
    public override float CooldownTime => cooldown;
    [SerializeField] private float castingTime; 
    public override float CastingTime => castingTime;

    public GameObject SummonEffect;
    public EnemyBase SummonBase;
    public int Count;

    public override void Resolve(EcsWorld world, BattleState state, int entityCaster)
    {
        ref var casterTransformComp = ref world.GetPool<TransformComponent>().Get(entityCaster);
        Transform tr = casterTransformComp.Transform;

        Vector3 forward = tr.forward;
        Vector3 right = tr.right;

        float offsetForward = 1.5f;
        float spread = 1.0f;

        for (int i = 0; i < Count; i++)
        {
             
            float t = (Count == 1) ? 0f : Mathf.Lerp(-1f, 1f, (float)i / (Count - 1));

            Vector3 spawnPoint =
                tr.position +
                forward * offsetForward + 
                right * (t * spread);       

            var spawnComp = new SpawnEvent()
            {
                SpawnPoint = spawnPoint,
                Amplifier = 1f
            };

            SummonBase.Init(world, state, ref spawnComp);


            var vfxEntity = -1;

            if (EntityPoolService.TryGet(SummonEffect.name, out vfxEntity))
            {
                ref var tc = ref world.GetPool<TransformComponent>().Get(vfxEntity);
                tc.Transform.position = spawnPoint;
                tc.Transform.gameObject.SetActive(true);
            }
            else
            {
                vfxEntity = world.NewEntity();

                ref var tc = ref world.GetPool<TransformComponent>().Add(vfxEntity);
                ref var vfx = ref world.GetPool<VFXComponent>().Add(vfxEntity);

                var instance = GameObject.Instantiate(SummonEffect, spawnPoint, Quaternion.identity);
                tc.Transform = instance.transform;
                tc.Transform.gameObject.SetActive(true);
            }

            world.GetPool<LifetimeComponent>().Add(vfxEntity).RemainingTime = 2f;
        }
    }
}
