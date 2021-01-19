using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct WeaponComponent :  IComponentData
{
    public float FireTime;
    public float FireRate;
    public int MaxAmmo;
    public int CurrentAmmo;
    public float ReloadTime;
    public ProjectileComponent Projectile;
    public Entity ProjectilePrefab;

    
}
