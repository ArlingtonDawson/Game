using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerRotationSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        var layerMask = LayerMask.GetMask("Floor");

        if (Physics.Raycast(cameraRay, out var hit, 100, layerMask))
        {
            //Entities.ForEach((Transform transform, RotationComponent rotationComponet) =>
            //{
            //    var foward = hit.point - transform.position;
            //    var rotation = Quaternion.LookRotation(foward);

            //    rotationComponet.Value = new Quaternion(0, rotation.y, 0, rotation.w).normalized;
            //});
        }
    }
}

