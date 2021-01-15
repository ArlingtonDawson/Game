using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Physics;
using Unity.Transforms;
using Unity.Mathematics;

public class PlayerMovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;

        Entities.ForEach((ref InputComponent inputComponent,ref PhysicsVelocity velocity, ref MovementComponent movement, ref Rotation rotation) =>
        {
            //Please let me know if there is another way to achive what is being done here. Tried my best to cut down on conversions.
            float4 lRotation = rotation.Value.value;
            Quaternion covertedQuaternion = new Quaternion(lRotation.x, lRotation.y, lRotation.z, lRotation.w);
            Vector3 targetForwardBack = (covertedQuaternion * Vector3.forward) * (movement.MoveSpeed * inputComponent.FowardBack) * deltaTime;
            Vector3 targetLeftRight = (covertedQuaternion * Vector3.left) * (movement.MoveSpeed * inputComponent.LeftRight) * deltaTime;
            Vector3 combindedVelocity = targetForwardBack + targetLeftRight;

            velocity.Linear += math.float3(combindedVelocity.x, combindedVelocity.y, combindedVelocity.z);
        });
    }
}
