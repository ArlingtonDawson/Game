using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class NewSystem : SystemBase
{
    protected override void OnUpdate()
    {
        int spawnerCount = GetEntityQuery(ComponentType.ReadOnly<PlayerComponent>()).CalculateEntityCount();
        if (spawnerCount == 0)
        {
            Entities.WithStructuralChanges().ForEach((in PlayerPrefabComponent playerPrefab) =>
            {
                Entity newSpawner = EntityManager.Instantiate(playerPrefab.Prefab);
                EntityManager.SetComponentData(newSpawner,
                    new Translation
                    {
                        Value = new float3(0, 0, 0)
                    });
            }).Run();
        }
    }
}
