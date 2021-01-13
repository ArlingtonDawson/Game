using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct SpawnerComponent : IComponentData
{
    public float SpawnTimer;
    public Entity TargetType;
    public float Rate;
}
