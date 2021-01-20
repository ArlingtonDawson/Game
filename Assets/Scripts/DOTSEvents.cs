using System;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;

public class DOTSEvents_NextFrame<T> where T : struct, IComponentData
{

    private World world;
    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;
    private EntityManager entityManager;
    private EntityArchetype eventEntityArchetype;
    private EntityQuery eventEntityQuery;
    private Action<T> OnEventAction;

    public DOTSEvents_NextFrame(World world, Action<T> OnEventAction = null)
    {
        this.world = world;
        this.OnEventAction = OnEventAction;
        endSimulationEntityCommandBufferSystem = world.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        entityManager = world.EntityManager;

        eventEntityArchetype = entityManager.CreateArchetype(typeof(T));
        eventEntityQuery = entityManager.CreateEntityQuery(typeof(T));
    }

    public EventTrigger GetEventTrigger()
    {
        return new EventTrigger(eventEntityArchetype, endSimulationEntityCommandBufferSystem.CreateCommandBuffer());
    }

    public EventTrigger_NotConcurrent GetEventTriggerNotConcurrent()
    {
        return new EventTrigger_NotConcurrent(eventEntityArchetype, endSimulationEntityCommandBufferSystem.CreateCommandBuffer());
    }

    public void CaptureEvents(EventTrigger eventTrigger, JobHandle jobHandleWhereEventsWereScheduled, Action<T> OnEventAction = null)
    {
        endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandleWhereEventsWereScheduled);
        eventTrigger.Playback(endSimulationEntityCommandBufferSystem.CreateCommandBuffer(), eventEntityQuery, OnEventAction == null ? this.OnEventAction : OnEventAction);
    }

    public void CaptureEvents(EventTrigger_NotConcurrent eventTrigger, JobHandle jobHandleWhereEventsWereScheduled, Action<T> OnEventAction = null)
    {
        endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandleWhereEventsWereScheduled);
        eventTrigger.Playback(endSimulationEntityCommandBufferSystem.CreateCommandBuffer(), eventEntityQuery, OnEventAction == null ? this.OnEventAction : OnEventAction);
    }



    public struct EventTrigger
    {

        private struct EventJob : IJobForEachWithEntity<T>
        {
            public EntityCommandBuffer.ParallelWriter entityCommandBufferConcurrent;
            public NativeList<T> nativeList;

            public void Execute(Entity entity, int index, ref T c0)
            {
                nativeList.Add(c0);
                entityCommandBufferConcurrent.DestroyEntity(index, entity);
            }
        }

        private EntityCommandBuffer.ParallelWriter entityCommandBufferConcurrent;
        private EntityArchetype entityArchetype;

        public EventTrigger(EntityArchetype entityArchetype, EntityCommandBuffer entityCommandBuffer)
        {
            this.entityArchetype = entityArchetype;
            entityCommandBufferConcurrent = entityCommandBuffer.AsParallelWriter();
        }

        public void TriggerEvent(int entityInQueryIndex)
        {
            entityCommandBufferConcurrent.CreateEntity(entityInQueryIndex, entityArchetype);
        }

        public void TriggerEvent(int entityInQueryIndex, T t)
        {
            Entity entity = entityCommandBufferConcurrent.CreateEntity(entityInQueryIndex, entityArchetype);
            entityCommandBufferConcurrent.SetComponent(entityInQueryIndex, entity, t);
        }


        public void Playback(EntityCommandBuffer destroyEntityCommandBuffer, EntityQuery eventEntityQuery, Action<T> OnEventAction)
        {
            if (eventEntityQuery.CalculateEntityCount() > 0)
            {
                NativeList<T> nativeList = new NativeList<T>(Allocator.TempJob);
                new EventJob
                {
                    entityCommandBufferConcurrent = destroyEntityCommandBuffer.AsParallelWriter(),
                    nativeList = nativeList,
                }.Run(eventEntityQuery);

                foreach (T t in nativeList)
                {
                    OnEventAction(t);
                }

                nativeList.Dispose();
            }
        }

    }

    public struct EventTrigger_NotConcurrent
    {

        private struct EventJob : IJobForEachWithEntity<T>
        {
            public EntityCommandBuffer.ParallelWriter entityCommandBufferConcurrent;
            public NativeList<T> nativeList;

            public void Execute(Entity entity, int index, ref T c0)
            {
                nativeList.Add(c0);
                entityCommandBufferConcurrent.DestroyEntity(index, entity);
            }
        }

        private EntityCommandBuffer entityCommandBuffer;
        private EntityArchetype entityArchetype;

        public EventTrigger_NotConcurrent(EntityArchetype entityArchetype, EntityCommandBuffer entityCommandBuffer)
        {
            this.entityArchetype = entityArchetype;
            this.entityCommandBuffer = entityCommandBuffer;
        }

        public void TriggerEvent()
        {
            entityCommandBuffer.CreateEntity(entityArchetype);
        }

        public void TriggerEvent(T t)
        {
            Entity entity = entityCommandBuffer.CreateEntity(entityArchetype);
            entityCommandBuffer.SetComponent(entity, t);
        }


        public void Playback(EntityCommandBuffer destroyEntityCommandBuffer, EntityQuery eventEntityQuery, Action<T> OnEventAction)
        {
            if (eventEntityQuery.CalculateEntityCount() > 0)
            {
                NativeList<T> nativeList = new NativeList<T>(Allocator.TempJob);
                new EventJob
                {
                    entityCommandBufferConcurrent = destroyEntityCommandBuffer.AsParallelWriter(),
                    nativeList = nativeList,
                }.Run(eventEntityQuery);

                foreach (T t in nativeList)
                {
                    OnEventAction(t);
                }

                nativeList.Dispose();
            }
        }

    }

}

public class DOTSEvents_SameFrame<T> where T : struct, IComponentData
{

    private World world;
    private EntityManager entityManager;
    private EntityArchetype eventEntityArchetype;
    private EntityQuery eventEntityQuery;
    private Action<T> OnEventAction;

    private EntityCommandBuffer entityCommandBuffer;

    public DOTSEvents_SameFrame(World world, Action<T> OnEventAction = null)
    {
        this.world = world;
        this.OnEventAction = OnEventAction;
        entityManager = world.EntityManager;

        eventEntityArchetype = entityManager.CreateArchetype(typeof(T));
        eventEntityQuery = entityManager.CreateEntityQuery(typeof(T));
    }

    public EventTrigger GetEventTrigger()
    {
        return new EventTrigger(eventEntityArchetype, out entityCommandBuffer);
    }

    public EventTrigger_NotConcurrent GetEventTriggerNotConcurrent()
    {
        return new EventTrigger_NotConcurrent(eventEntityArchetype);
    }

    public void CaptureEvents(EventTrigger eventTrigger, JobHandle jobHandleWhereEventsWereScheduled, Action<T> OnEventAction = null)
    {
        eventTrigger.Playback(jobHandleWhereEventsWereScheduled, entityCommandBuffer, entityManager, eventEntityQuery, OnEventAction == null ? this.OnEventAction : OnEventAction);
    }

    public void CaptureEvents(EventTrigger_NotConcurrent eventTriggerNotConcurrent, JobHandle jobHandleWhereEventsWereScheduled, Action<T> OnEventAction = null)
    {
        eventTriggerNotConcurrent.Playback(jobHandleWhereEventsWereScheduled, entityManager, eventEntityQuery, OnEventAction == null ? this.OnEventAction : OnEventAction);
    }



    public struct EventTrigger
    {

        private EntityCommandBuffer.ParallelWriter entityCommandBufferConcurrent;
        private EntityArchetype entityArchetype;

        public EventTrigger(EntityArchetype entityArchetype, out EntityCommandBuffer entityCommandBuffer)
        {
            this.entityArchetype = entityArchetype;

            entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
            entityCommandBufferConcurrent = entityCommandBuffer.AsParallelWriter();
        }

        public void TriggerEvent(int entityInQueryIndex)
        {
            entityCommandBufferConcurrent.CreateEntity(entityInQueryIndex, entityArchetype);
        }

        public void TriggerEvent(int entityInQueryIndex, T t)
        {
            Entity entity = entityCommandBufferConcurrent.CreateEntity(entityInQueryIndex, entityArchetype);
            entityCommandBufferConcurrent.SetComponent(entityInQueryIndex, entity, t);
        }


        public void Playback(JobHandle jobHandleWhereEventsWereScheduled, EntityCommandBuffer entityCommandBuffer, EntityManager EntityManager, EntityQuery eventEntityQuery, Action<T> OnEventAction)
        {
            jobHandleWhereEventsWereScheduled.Complete();
            entityCommandBuffer.Playback(EntityManager);
            entityCommandBuffer.Dispose();

            int entityCount = eventEntityQuery.CalculateEntityCount();
            if (entityCount > 0)
            {
                NativeArray<T> nativeArray = eventEntityQuery.ToComponentDataArray<T>(Allocator.TempJob);
                foreach (T t in nativeArray)
                {
                    OnEventAction(t);
                }
                nativeArray.Dispose();
            }

            EntityManager.DestroyEntity(eventEntityQuery);
        }

    }

    public struct EventTrigger_NotConcurrent
    {

        private EntityCommandBuffer entityCommandBuffer;
        private EntityArchetype entityArchetype;

        public EventTrigger_NotConcurrent(EntityArchetype entityArchetype)
        {
            this.entityArchetype = entityArchetype;
            entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
        }

        public void TriggerEvent()
        {
            entityCommandBuffer.CreateEntity(entityArchetype);
        }

        public void TriggerEvent(T t)
        {
            Entity entity = entityCommandBuffer.CreateEntity(entityArchetype);
            entityCommandBuffer.SetComponent(entity, t);
        }

        public void Playback(JobHandle jobHandleWhereEventsWereScheduled, EntityManager EntityManager, EntityQuery eventEntityQuery, Action<T> OnEventAction)
        {
            jobHandleWhereEventsWereScheduled.Complete();
            entityCommandBuffer.Playback(EntityManager);
            entityCommandBuffer.Dispose();

            int entityCount = eventEntityQuery.CalculateEntityCount();
            if (entityCount > 0)
            {
                NativeArray<T> nativeArray = eventEntityQuery.ToComponentDataArray<T>(Allocator.TempJob);
                foreach (T t in nativeArray)
                {
                    OnEventAction(t);
                }
                nativeArray.Dispose();
            }

            EntityManager.DestroyEntity(eventEntityQuery);
        }

    }

}
