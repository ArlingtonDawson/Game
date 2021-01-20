using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static AmmoDisplayHandler;

public class WeaponReloadSystem : SystemBase
{
    public event EventHandler<OnAmmoDiplayChangeArgs> OnAmmoChange;
    public struct OnAmmoChangedEvent : IComponentData { public int CurrentAmmo; public int MaxAmmo; }
    public DOTSEvents_NextFrame<OnAmmoChangedEvent> dotsEvents;

    public EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem { get; private set; }

    protected override void OnCreate()
    {
        base.OnCreate();
        // Find the ECB system once and store it for later usage
        m_EndSimulationEcbSystem = World
            .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        dotsEvents = new DOTSEvents_NextFrame<OnAmmoChangedEvent>(World);
    }

    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();
        DOTSEvents_NextFrame<OnAmmoChangedEvent>.EventTrigger eventTrigger = dotsEvents.GetEventTrigger();

        Entities.ForEach((Entity entity, int nativeThreadIndex, ref WeaponComponent weapon, ref ReloadComponent reload) => {
            reload.ReloadTime -= deltaTime;
            if(reload.ReloadTime < 0)
            {
                weapon.CurrentAmmo = weapon.MaxAmmo;
                ecb.RemoveComponent<ReloadComponent>(nativeThreadIndex, entity);

                eventTrigger.TriggerEvent(nativeThreadIndex, new OnAmmoChangedEvent { CurrentAmmo = weapon.CurrentAmmo, MaxAmmo = weapon.MaxAmmo });
            }
        }).Schedule();

        dotsEvents.CaptureEvents(eventTrigger, this.Dependency, (OnAmmoChangedEvent onAmmoChangedEvent) => {
            OnAmmoChange?.Invoke(this, new OnAmmoDiplayChangeArgs { CurrentAmmo = onAmmoChangedEvent.CurrentAmmo, MaxAmmo = onAmmoChangedEvent.MaxAmmo});
        });

        m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
