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
        var ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();
        JobHandle jobHandle;

        if(Input.GetKey(KeyCode.Space))
        {
            Entities.WithAll<WeaponComponent>().WithNone<Firing>().ForEach((Entity e, int nativeThreadIndex) =>
                {
                    ecb.AddComponent(nativeThreadIndex, e, new Firing());
                }).ScheduleParallel();
        } else
        {
            Entities.WithAll<WeaponComponent, Firing>().ForEach((Entity e, int nativeThreadIndex) =>
            {
                ecb.RemoveComponent(nativeThreadIndex, e, typeof(Firing));
            }).ScheduleParallel();
        }

        m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
