using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class PlayerRotationSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;

        Entities.WithAll<PlayerComponent>().ForEach((ref InputComponent inputComponent, ref Rotation rotation) =>
        {
            rotation.Value *= Quaternion.Euler(inputComponent.YRotation, inputComponent.XRotation, 0);
        });
    }
}

