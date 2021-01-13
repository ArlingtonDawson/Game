using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct MovementComponent : IComponentData
{
    public float MoveSpeed;
}
