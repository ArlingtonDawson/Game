using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct AmmoCountUpdate : IComponentData
{
    public int Current;
    public int Max;
}
