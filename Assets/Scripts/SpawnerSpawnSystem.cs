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
        var deltaTime = Time.DeltaTime;
        Entity player = GetSingletonEntity<PlayerComponent>();

        Entities.WithStructuralChanges().ForEach((ref SpawnerComponent spawner, ref Translation translation) =>
        {
            spawner.SpawnTimer -= deltaTime;

            if(spawner.SpawnTimer < 0)
            {
                spawner.SpawnTimer = spawner.Rate;
                Entity newSpawner = EntityManager.Instantiate(spawner.TargetType);
                EntityManager.SetComponentData(newSpawner,
                    new Translation
                    {
                        Value = translation.Value + new float3(0,0,5) //Temp fix to test some issues. Fix this to use the size of the spawner.
                    });
                EntityManager.SetComponentData(newSpawner,
                    new TargetComponent
                    {
                        Entity = player
                    });
            }

        }).Run();
     }

}
