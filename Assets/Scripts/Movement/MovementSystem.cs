using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;

        Entities.WithNone<InputComponent>().ForEach((ref PhysicsVelocity velocity, ref MovementComponent movement, ref Rotation rotation) =>
        {
            float4 lRotation = rotation.Value.value;
            Quaternion covertedQuaternion = new Quaternion(lRotation.x, lRotation.y, lRotation.z, lRotation.w);
            Vector3 targetForwardBack = (covertedQuaternion * Vector3.forward) * (movement.MoveSpeed) * deltaTime;

            velocity.Linear += math.float3(targetForwardBack.x, targetForwardBack.y, targetForwardBack.z);
        }).Run();
    }
}
