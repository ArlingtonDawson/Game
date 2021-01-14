using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;

[Serializable]
[GenerateAuthoringComponent]
public struct InputComponent : IComponentData
{
    public float Horizontal;
    public float Vertical;
    public float XRotation;
    public float YRotation;
}
