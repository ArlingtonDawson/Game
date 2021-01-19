using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Collider = Unity.Physics.Collider;

public class WeaponFiringSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        // Find the ECB system once and store it for later usage
        m_EndSimulationEcbSystem = World
            .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();

        Entities.WithNone<ReloadComponent>().ForEach((int nativeThreadIndex, LocalToWorld world, ref Firing firing, ref WeaponComponent weapon) => {
            if(weapon.CurrentAmmo > 0)
            {
                firing.FireCountDown -= deltaTime;

                if(firing.FireCountDown < 0)
                {
                    firing.FireCountDown = weapon.FireRate;
                   
                    Entity newProjectile = ecb.Instantiate(nativeThreadIndex, weapon.ProjectilePrefab);
                    
                    ecb.SetComponent(nativeThreadIndex, newProjectile, new Translation { Value = world.Position });
                    ecb.SetComponent(nativeThreadIndex, newProjectile, new Rotation { Value = world.Rotation });
                    ecb.SetComponent(nativeThreadIndex, newProjectile, new PhysicsVelocity
                    {
                        Linear = new float3(0,0, weapon.Projectile.Velocity),
                        Angular = float3.zero
                    });
                    Debug.Log("Create Projectile");

                    weapon.CurrentAmmo--;
                }
            }
        }).Schedule();

        m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
    }

    public static void CreateProjectileEntity(EntityCommandBuffer.ParallelWriter ecb, int nativeThreadIndex, ProjectileComponent projectile, Rotation rotation, Translation translation)
    {
        //I really would like to see this working. How do I load meshes? Its a sphere, is there a constant in unity?
        //Second problem I am having is that the rotation and transform are coming back not a number. I figure this is because I may be missing some render componets.
        BlobAssetReference<Unity.Physics.Collider> spCollider = Unity.Physics.SphereCollider.Create(new Unity.Physics.SphereGeometry
        {
            Center = float3.zero,
            Radius = 4
        });

        Entity newProjectile =  ecb.CreateEntity(nativeThreadIndex);
        ecb.AddComponent(nativeThreadIndex, newProjectile, projectile.Damage);
        //Physics Velocity
        ecb.AddComponent(nativeThreadIndex, newProjectile, new PhysicsVelocity { 
            Linear = new float3(0,0,projectile.Velocity),
            Angular = float3.zero
        });
        ecb.AddSharedComponent(nativeThreadIndex, newProjectile, new RenderMesh());
        //Physics Mass
        ecb.AddComponent(nativeThreadIndex, newProjectile, PhysicsMass.CreateDynamic(new MassProperties(), 1000));
        ecb.AddComponent(nativeThreadIndex, newProjectile, new Unity.Physics.PhysicsCollider { 
            Value = spCollider});
        ecb.AddComponent(nativeThreadIndex, newProjectile, new Rotation { 
            Value = rotation.Value});
        ecb.AddComponent(nativeThreadIndex, newProjectile, new Translation { 
            Value = translation.Value});
    }
}
