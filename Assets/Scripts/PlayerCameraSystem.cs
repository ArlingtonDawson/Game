using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class PlayerCameraSystem : SystemBase
{
    protected override void OnUpdate()
    {
        //Is there a better way?
        Entity player = GetSingletonEntity<PlayerComponent>();
        Translation playerTranslation = GetComponent<Translation>(player);
        Rotation playerRotation = GetComponent<Rotation>(player);

        Entities
            .WithAll<Camera>()
            .ForEach((Transform transform, ref Translation translation, ref Rotation rotation) =>
            {
                transform.position = playerTranslation.Value;
                transform.rotation = playerRotation.Value;
            }).WithoutBurst().Run();
    }
}
