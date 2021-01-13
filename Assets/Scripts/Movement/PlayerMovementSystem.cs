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

        Entities.ForEach((ref InputComponent inputComponent, ref PhysicsVelocity velocity, ref MovementComponent movement) =>
        {
            float2 newVelocity = velocity.Linear.xz;
            float2 currentInput = new float2(inputComponent.Vertical, inputComponent.Horizontal);
            velocity.Linear.xz += currentInput * movement.MoveSpeed * deltaTime;
        });
    }
}
