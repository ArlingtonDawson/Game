using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct WeaponComponent :  IComponentData
{
    public float FireTime;
    public float FireRate;
    public int MaxAmmo;
    public int CurrentAmmo;
    public ProjectileComponent Projectile;
}
