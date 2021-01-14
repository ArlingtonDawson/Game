using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class PlayerRotationSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;

        Entities.ForEach((ref InputComponent inputComponent, ref PhysicsVelocity velocity) =>
        {
            velocity.Angular.y = inputComponent.XRotation;
            velocity.Angular.x = inputComponent.YRotation;
        });
    }
}

