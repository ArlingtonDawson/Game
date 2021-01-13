using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class SpawnerSpawnSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.WithStructuralChanges().ForEach((ref SpawnerComponent spawner, ref Translation translation) =>
        {
            Entity newSpawner = EntityManager.Instantiate(spawner.TargetType);
            EntityManager.SetComponentData(newSpawner,
                new Translation
                {
                    Value = translation.Value
                });
        }).Run();
     }

}
