using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct DamageComponent : IComponentData
{
    public float Value;
    public int Type;
}
