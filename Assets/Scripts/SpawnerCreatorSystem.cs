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
        
        //TODO: FIX THIS TO MAKE IT A BETTER SEED SYSTEM.
        System.DateTime date = System.DateTime.Now;
        
        random = new Random(System.UInt16.Parse((date.Month + date.Day + date.Hour + date.Minute + date.Second).ToString()));
    }
    protected override void OnUpdate()
    {
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
