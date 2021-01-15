using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class InputSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var fowardBack = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
        var leftRight = (Input.GetKey(KeyCode.A) ? 1 : 0) - (Input.GetKey(KeyCode.D) ? 1 : 0);
        var xRotation = Input.GetAxisRaw("Mouse X");
        var yRotation = Input.GetAxisRaw("Mouse Y");

        Entities.ForEach((ref InputComponent inputComponent) =>
        {
            inputComponent.FowardBack = fowardBack;
            inputComponent.LeftRight = leftRight;
            inputComponent.XRotation = xRotation;
            inputComponent.YRotation = yRotation;
        });
    }
}
