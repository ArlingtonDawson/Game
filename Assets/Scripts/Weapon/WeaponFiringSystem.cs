using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class WeaponFiringSystem : SystemBase
{
    protected override void OnUpdate()
    {   
        var deltaTime = Time.DeltaTime;
        Entities.ForEach((ref Firing firing, ref WeaponComponent weapon) => {
            if(weapon.CurrentAmmo > 0)
            {
                firing.FireCountDown -= deltaTime;

                if(firing.FireCountDown < 0)
                {
                    firing.FireCountDown = weapon.FireRate;
                    Debug.Log("Create Projectile");
                    weapon.CurrentAmmo--;
                }
            }
        }).Schedule();
    }
}
