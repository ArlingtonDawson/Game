using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class SpawnerCreatorSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
    Random random;
    protected override void OnCreate()
    {
        base.OnCreate();
        // Find the ECB system once and store it for later usage
        m_EndSimulationEcbSystem = World
            .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        
        //TODO: FIX THIS TO MAKE IT A BETTER SEED SYSTEM.
        System.DateTime date = System.DateTime.Now;
        
        random = new Random(System.UInt16.Parse((date.Month + date.Day + date.Hour + date.Minute + date.Second).ToString()));
    }
    protected override void OnUpdate()
    {


        //NOTE TO SELF: KEEPING THIS HERE FOR THE TIME BEING. While this has no use here, I need it for a later portion and I just dont want to do a good search to find it again.
        //var ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();
        //m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
        Random lRandom = random;
        int spawnerCount = GetEntityQuery(ComponentType.ReadOnly<SpawnerComponent>()).CalculateEntityCount();
        if(spawnerCount == 0)
        {
            Entities.WithStructuralChanges().ForEach((in SpawnerPrefabComponent spawnerPrefab) =>
            {
                Entity newSpawner = EntityManager.Instantiate(spawnerPrefab.Prefab);
                EntityManager.SetComponentData(newSpawner,
                    new Translation { 
                        Value = new float3(lRandom.NextFloat(-10f,10f), 0, lRandom.NextFloat(-10f, 10f))
                    });
            }).Run();
        }
    }
}
