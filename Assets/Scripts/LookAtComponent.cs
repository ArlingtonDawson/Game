using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct LookAtComponent : IComponentData
{
    public Quaternion Value;
}
