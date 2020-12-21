using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[UpdateInGroup(typeof(GameObjectConversionGroup))]
public class PlayerConversion : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities
            .ForEach((PlayerAuthoring playerAuthoring) =>
            {
                var entity = GetPrimaryEntity(playerAuthoring);
                var cameraFollowComponent = new CameraFollowComponent
                {
                    CameraFollowOffset = new float3(0, 10, -10),
                    CameraSmoothSpeed = 0.25f
                };

                DstEntityManager.AddComponent(entity, typeof(MovementComponent));
                DstEntityManager.AddComponentData(entity, cameraFollowComponent);
            });
    }

}
