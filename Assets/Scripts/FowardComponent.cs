﻿using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct FowardComponent : IComponentData
{
    public Quaternion Foward;
}
