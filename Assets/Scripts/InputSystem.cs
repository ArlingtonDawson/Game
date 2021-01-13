using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class InputSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var horizontal = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
        var vertical = (Input.GetKey(KeyCode.A) ? 1 : 0) - (Input.GetKey(KeyCode.D) ? 1 : 0);

        Entities.ForEach((ref InputComponent inputComponent) =>
        {
            inputComponent.Horizontal = horizontal;
            inputComponent.Vertical = vertical;
        });
    }
}
