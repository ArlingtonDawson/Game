using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;

        Entities.WithNone<InputComponent>().ForEach((ref PhysicsVelocity velocity, ref MovementComponent movement) =>
        {
            velocity.Linear.z += movement.MoveSpeed * deltaTime;
        }).Run();
    }
}
