using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class WeaponReloadSystem : SystemBase
{
    public EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem { get; private set; }

    protected override void OnCreate()
    {
        base.OnCreate();
        // Find the ECB system once and store it for later usage
        m_EndSimulationEcbSystem = World
            .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();

        Entities.ForEach((Entity entity, int nativeThreadIndex, ref WeaponComponent weapon, ref ReloadComponent reload) => {
            reload.ReloadTime -= deltaTime;
            if(reload.ReloadTime < 0)
            {
                ecb.RemoveComponent<ReloadComponent>(nativeThreadIndex, entity);
            }
        }).Schedule();

        m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
