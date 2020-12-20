using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(PlayerInputSystem))]
public class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithoutBurst()
            .ForEach(
            (ref Rotation rotation, ref Translation translation, ref MovementComponent movementComponent) =>
            {
                if (movementComponent.moveRequested)
                {
                    var forwardVector = math.forward(rotation.Value);

                    if (math.dot(forwardVector, movementComponent.moveDirection) < 0.9)
                    {
                        var rotationValue = quaternion.LookRotation(movementComponent.moveDirection, new float3(0, 1, 0));
                        var newForward = math.forward(rotationValue);
                        rotation.Value = rotationValue;
                    }
                    else
                    {
                        translation.Value += (movementComponent.moveDirection * 1f);
                    }
                    movementComponent.moveRequested = false;
                }
            })
            .Run();
    }
}
