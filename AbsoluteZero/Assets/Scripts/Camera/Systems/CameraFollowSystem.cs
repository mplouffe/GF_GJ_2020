using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(MovementSystem))]
public class CameraFollowSystem : SystemBase
{
    private EntityQuery m_LocalPlayer;

    protected override void OnCreate()
    {
        m_LocalPlayer = GetEntityQuery(ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<CameraFollowComponent>());
    }

    protected override void OnUpdate()
    {
        var localPlayerEntity = m_LocalPlayer.GetSingletonEntity();
        var localPlayerTranslation = GetComponent<Translation>(localPlayerEntity);
        var cameraFollowComponent = GetComponent<CameraFollowComponent>(localPlayerEntity);



        Entities
            .WithAll<Camera>()
            .WithoutBurst()
            .ForEach((Transform transform) =>
            {
                float3 desiredPosition = localPlayerTranslation.Value + cameraFollowComponent.CameraFollowOffset;
                float3 currentPosition = new float3(transform.position);
                float3 smoothedPosition = math.lerp(currentPosition, desiredPosition, cameraFollowComponent.CameraSmoothSpeed);
                transform.position = smoothedPosition;
            })
            .Run();
    }
}
