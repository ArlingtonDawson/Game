using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct ProjectileComponent : IComponentData
{
    public float Velocity;
    public DamageComponent Damage;
}
