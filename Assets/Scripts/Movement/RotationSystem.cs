using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class RotationSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;

        Entities.WithNone<PlayerComponent>().ForEach((ref Rotation rotation, ref Translation postion, ref TargetComponent target, ref TurnRateComponent turnRate) =>
        {
            ComponentDataFromEntity<Translation> translationArray = GetComponentDataFromEntity<Translation>();

            if(!translationArray.HasComponent(target.Entity))
            {
                return;
            }


            float3 directionToTarget = translationArray[target.Entity].Value - postion.Value;
            quaternion targetRotation = quaternion.LookRotationSafe(directionToTarget, math.up());
            rotation.Value = math.slerp(rotation.Value, targetRotation, turnRate.Rate * deltaTime);
        });
    }
}

