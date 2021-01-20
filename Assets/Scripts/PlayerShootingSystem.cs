using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class PlayerShootingSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
    EntityQueryDesc EntityQueryDesc;

    protected override void OnCreate()
    {
        base.OnCreate();
        // Find the ECB system once and store it for later usage
        m_EndSimulationEcbSystem = World
            .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();
        Entity player = GetSingletonEntity<PlayerComponent>();
        InputComponent playerInput = GetComponent<InputComponent>(player);

        if (playerInput.Fire)
        {
            Entities.WithAll<WeaponComponent>().WithNone<Firing, ReloadComponent>().ForEach((Entity e, int nativeThreadIndex) =>
                {
                    ecb.AddComponent(nativeThreadIndex, e, new Firing { FireCountDown = 0 });
                }).ScheduleParallel();
        } else
        {
            Entities.WithAll<WeaponComponent, Firing>().ForEach((Entity e, int nativeThreadIndex) =>
            {
                ecb.RemoveComponent(nativeThreadIndex, e, typeof(Firing));
            }).ScheduleParallel();
        }

        if (playerInput.Reload)
        {
            Entities.WithAll<WeaponComponent>().WithNone<ReloadComponent>().ForEach((Entity e, int nativeThreadIndex, WeaponComponent weapon) =>
            {
                ecb.AddComponent(nativeThreadIndex, e, new ReloadComponent { 
                    ReloadTime = weapon.ReloadTime
                });
            }).ScheduleParallel();
        }

        m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
