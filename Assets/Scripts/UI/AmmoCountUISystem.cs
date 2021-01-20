using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.UI;

public class AmmoCountUISystem : SystemBase
{
    protected override void OnUpdate()
    {
         Entities.ForEach((ref Translation translation) => {
             //text.text = "Hello";
        }).Run();
    }

    public static void UpdateAmmoCount(EntityCommandBuffer.ParallelWriter ecb, int nativeThreadIndex, WeaponComponent weapon)
    {
        Entity updateAmmo = ecb.CreateEntity(nativeThreadIndex);
        ecb.AddComponent<AmmoCountUpdate>(nativeThreadIndex, updateAmmo, new AmmoCountUpdate { Current = weapon.CurrentAmmo, Max = weapon.MaxAmmo });
    }
}
