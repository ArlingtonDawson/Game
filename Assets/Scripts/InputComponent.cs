using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;

[Serializable]
[GenerateAuthoringComponent]
public struct InputComponent : IComponentData
{
    public float FowardBack;
    public float LeftRight;
    public float XRotation;
    public float YRotation;
}
