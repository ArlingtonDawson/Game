using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct ResistanceComponent : IComponentData
{
    public float Value;
    public int Type;
}
